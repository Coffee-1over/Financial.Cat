namespace Financial.Cat.Domain.Models.Business.History
{
	public class BaseHistoryFilterModel : PaginationModel
	{
		public long? UserId { get; set; }
		public DateTime? Start { get; set; }
		public DateTime? End { get; set; }
	}
}
