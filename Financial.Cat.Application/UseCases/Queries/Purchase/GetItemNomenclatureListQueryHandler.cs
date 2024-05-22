using AutoMapper;
using Financial.Cat.Application.Accessors;
using Financial.Cat.Domain.Interfaces.Repositories;
using Financial.Cat.Domain.Models.Dto.Out.Purchase;
using Financial.Cat.Domain.Models.Queries.Purchase;
using MediatR;

namespace Financial.Cat.Application.UseCases.Queries.Purchase
{
	internal class GetItemNomenclatureListQueryHandler : IRequestHandler<GetItemNomenclatureListQuery, IList<ItemNomenclatureOutDto>>
	{
		private readonly IItemNomenclatureRepository _itemNomenclatureRepository;
		private readonly IUserContextAccessor _userContextAccessor;
		private readonly IMapper _mapper;

		public GetItemNomenclatureListQueryHandler(IItemNomenclatureRepository itemNomenclatureRepository, IUserContextAccessor userContextAccessor, IMapper mapper)
		{
			_itemNomenclatureRepository = itemNomenclatureRepository;
			_userContextAccessor = userContextAccessor;
			_mapper = mapper;
		}

		public async Task<IList<ItemNomenclatureOutDto>> Handle(GetItemNomenclatureListQuery request, CancellationToken cancellationToken)
		{
			if (request.OnlyMain)
				return await GetMaintemNomenclatureList(cancellationToken);

			var itemNomenclature = await _itemNomenclatureRepository.GetAsync(x => !x.IsHidden  || x.UserId == _userContextAccessor.UserContextModel.Id, cancellationToken);
			return _mapper.Map<IList<ItemNomenclatureOutDto>>(itemNomenclature);
		}

		private async Task<IList<ItemNomenclatureOutDto>> GetMaintemNomenclatureList(CancellationToken cancellationToken)
		{
			var itemNomenclature = await _itemNomenclatureRepository.GetAsync(x => x.UserId == _userContextAccessor.UserContextModel.Id, cancellationToken);
			return _mapper.Map<IList<ItemNomenclatureOutDto>>(itemNomenclature);
		}
	}
}
