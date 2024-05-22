using AutoMapper;
using Financial.Cat.Application.Accessors;
using Financial.Cat.Domain.Interfaces.Repositories;
using Financial.Cat.Domain.Models.Business.History;
using Financial.Cat.Domain.Models.Dto.Out.Purchase;
using Financial.Cat.Domain.Models.Queries.Purchase;
using MediatR;
using X.PagedList;

namespace Financial.Cat.Application.UseCases.Queries.Purchase
{
	public class GetPurchaseListQueryHandler : IRequestHandler<GetPurchaseListQuery, HistoryListModel<PurchaseOutDto>>
	{
		private readonly IPurchaseRepository _purchaseRepository;
		private readonly IUserContextAccessor _userContextAccessor;
		private readonly IMapper _mapper;

		public GetPurchaseListQueryHandler(IPurchaseRepository purchaseRepository, IUserContextAccessor userContextAccessor, IMapper mapper)
		{
			_purchaseRepository = purchaseRepository;
			_userContextAccessor = userContextAccessor;
			_mapper = mapper;
		}
		public async Task<HistoryListModel<PurchaseOutDto>> Handle(GetPurchaseListQuery request, CancellationToken cancellationToken)
		{
			var purchasesQuery = _purchaseRepository.Query().Where(x => x.UserId == _userContextAccessor.UserContextModel.Id);

			if (request.End.HasValue && request.Start.HasValue)
				purchasesQuery = purchasesQuery.Where(x=> x.PurchaseTime < request.End.Value && x.PurchaseTime > request.Start.Value);

			var purchases = await purchasesQuery.OrderByDescending(x => x.Created)
				.Select(_mapper.Map<PurchaseOutDto>)
				.ToPagedListAsync(request.PageNumber, request.PageSize, null, cancellationToken);

			return new HistoryListModel<PurchaseOutDto>(purchases);
		}
	}
}
