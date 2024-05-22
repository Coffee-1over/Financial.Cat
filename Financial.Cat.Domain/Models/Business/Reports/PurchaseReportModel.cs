namespace Financial.Cat.Domain.Models.Business.Reports
{
    public class PurchaseReportModel
    {
		public string ItemNomenclatureName { get; set; }
		public string CategoryName { get; set; }

		public DateTime? PurchaseTime { get; set; }
		public decimal Price { get; set; }
		public decimal Quantity { get; set; }
        public decimal Total { get; set; }
       
    }
}
