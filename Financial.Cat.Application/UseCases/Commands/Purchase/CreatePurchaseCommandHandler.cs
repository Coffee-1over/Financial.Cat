using AutoMapper;
using Financial.Cat.Application.Accessors;
using Financial.Cat.Domain.Interfaces.Repositories;
using Financial.Cat.Domain.Models.Commands.Purchase;
using Financial.Cat.Domain.Models.Dto.Out.Purchase;
using Financial.Cat.Domain.Models.Entities;
using MediatR;
using NetTopologySuite.Geometries;

namespace Financial.Cat.Application.UseCases.Commands.Purchase
{
    internal class CreatePurchaseCommandHandler : IRequestHandler<CreatePurchaseCommand, PurchaseOutDto>
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IUserContextAccessor _userContextAccessor;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

		public CreatePurchaseCommandHandler(IPurchaseRepository purchaseRepository, IUserContextAccessor userContextAccessor, IMapper mapper, IMediator mediator)
		{
			_purchaseRepository = purchaseRepository;
			_userContextAccessor = userContextAccessor;
			_mapper = mapper;
			_mediator = mediator;
		}

		public async Task<PurchaseOutDto> Handle(CreatePurchaseCommand request, CancellationToken cancellationToken)
        {
            var purchase = CreatePurchase(request);
            var savingResult = await _purchaseRepository.AddAsync(purchase, cancellationToken);

            var result = await _purchaseRepository.GetOneAsync(x => x.Id == savingResult.Id, cancellationToken);

			var calculateCommand = new CalculatePeriodsAndSendEmailReportCommand
			{
				UserId = _userContextAccessor.UserContextModel.Id,
			};
			await _mediator.Send(calculateCommand, cancellationToken);

			return _mapper.Map<PurchaseOutDto>(result);
            
        }

        private PurchaseEntity CreatePurchase(CreatePurchaseCommand request)
        {
            var userId = _userContextAccessor.UserContextModel.Id;
            var purchase = new PurchaseEntity
            {
                PurchaseTime = request.PurchaseTime,
                UserId = userId,
                Items = request.Items.Select(x => new ItemEntity
                {
                    ItemNomenclatureId = x.ItemNomenclature.Id ?? 0,
                    Quantity = x.Quantity,
                    Price = x.Price,
                    ItemNomenclature = !x.ItemNomenclature.Id.HasValue ? new ItemNomenclatureEntity
                    {
                        Id = x.ItemNomenclature.Id ?? 0,
                        Name = x.ItemNomenclature.Name,
                        CategoryId = x.ItemNomenclature.Category.Id ?? 0,
                        UserId = userId,
                        Category = !x.ItemNomenclature.Category.Id.HasValue ? new CategoryEntity
                        {
                            Id = x.ItemNomenclature.Category.Id ?? 0,
                            Name = x.ItemNomenclature.Category.Name,
                            ParentCategoryId = x.ItemNomenclature.Category.ParentCategoryId
                        } : null,
                        IsHidden = x.ItemNomenclature.IsHidden
                    } : null
                }).ToList(),
                AddressId = request.Address?.Id,
                Address = request.Address != null && !request.Address.Id.HasValue ? new AddressEntity
                {
                    Id = request.Address.Id ?? 0,
                    Name = request.Address.Name,
                    Street1 = request.Address.Street1,
                    Zip = request.Address.Zip,
                    City = request.Address.City,
                    Point = request.Address.X.HasValue && request.Address.Y.HasValue ? new Point(request.Address.X.Value, request.Address.Y.Value) : null,
                    Country = request.Address.Country
                } : null
            };

            return purchase;
        }
    }
}
