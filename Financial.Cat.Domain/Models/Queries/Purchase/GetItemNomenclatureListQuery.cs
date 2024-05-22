using Financial.Cat.Domain.Models.Dto.Out.Purchase;
using MediatR;

namespace Financial.Cat.Domain.Models.Queries.Purchase
{
	public class GetItemNomenclatureListQuery : IRequest<IList<ItemNomenclatureOutDto>>
	{
		public bool OnlyMain { get; set; }
	}
}
