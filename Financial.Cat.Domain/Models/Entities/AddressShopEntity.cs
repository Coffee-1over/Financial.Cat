namespace Financial.Cat.Domain.Models.Entities
{
	public class AddressShopEntity
	{
		public long AddressId { get; set; }
		public long ShopId { get; set; }
		public AddressEntity Address { get; set; }
		public ShopEntity Shop { get; set; }
	}
}
