using Financial.Cat.Domain.Models.Entities.Abstract;

namespace Financial.Cat.Domain.Models.Entities
{
	public class ShopEntity : IBaseEntity
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public ICollection<AddressShopEntity> AddressesShops { get; set; } = new List<AddressShopEntity>();
	}
}
