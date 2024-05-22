using Financial.Cat.Domain.Models.Entities.Abstract;
using NetTopologySuite.Geometries;

namespace Financial.Cat.Domain.Models.Entities
{
	public class AddressEntity : IBaseEntity, IAuditDateInfo
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string? Street1 { get; set; }
		public string? Zip { get; set; }
		public string? City { get; set; }
		public Point? Point { get; set; }
		public string Country { get; set; } = null!;

		public ICollection<AddressShopEntity> AddressesShops { get; set; } = new List<AddressShopEntity>();
		public ICollection<PurchaseEntity> Purchases { get; set; } = new List<PurchaseEntity>();

		public DateTime Created { get; set; }
		public DateTime Updated { get; set; }
	}
}
