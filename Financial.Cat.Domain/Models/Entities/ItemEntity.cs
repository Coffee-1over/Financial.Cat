using Financial.Cat.Domain.Models.Entities.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Financial.Cat.Domain.Models.Entities
{
	public class ItemEntity : IBaseEntity
	{
		public long Id { get; set; }

		public long PurchaseId { get; set; }
		public long ItemNomenclatureId { get; set; }
		public ItemNomenclatureEntity ItemNomenclature { get; set; }

		public decimal Quantity { get; set; }
		public decimal Price { get; set; }

		public decimal TotalPrice => Quantity * Price;

		public PurchaseEntity Purchase { get; set; }
	}
}
