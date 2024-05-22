using X.PagedList;

namespace Financial.Cat.Domain.Models.Business.History
{
    public class HistoryListModel<T>
    {
        public HistoryListModel(IPagedList<T> historyList)
        {
            HistoryList = historyList;
            PageCount = historyList.PageCount;
            TotalItemCount = historyList.TotalItemCount;
        }
        public IPagedList<T> HistoryList { get; set; }
        public int PageCount { get; set; }
        public int TotalItemCount { get; set; }
    }
}
