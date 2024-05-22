using Financial.Cat.Domain.Models.Business.History;
using Financial.Cat.Domain.Models.Dto.Out.Purchase;
using MediatR;

namespace Financial.Cat.Domain.Models.Queries.Purchase
{
	public class GetPurchaseListQuery : BaseHistoryFilterModel, IRequest<HistoryListModel<PurchaseOutDto>>
	{
		//write constructor with BaseHistoryFilterModel parameter

		public GetPurchaseListQuery() { }
		
		public GetPurchaseListQuery(BaseHistoryFilterModel model) 
		{
			UserId = model.UserId;
			Start = model.Start;
			End = model.End;
			PageNumber = model.PageNumber;
			PageSize = model.PageSize;
		}
	}
}
