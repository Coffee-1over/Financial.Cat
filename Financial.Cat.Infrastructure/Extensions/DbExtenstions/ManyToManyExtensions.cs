using Financial.Cat.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financial.Cat.Infrustructure.Extensions.DbExtenstions
{
	internal static class ManyToManyExtensions
	{
		public static ModelBuilder ConfigureManyToManyEntities(this ModelBuilder modelBuilder)
		{
			return modelBuilder.ConfigureManyToManyAddressShopEntity();
		}

		private static ModelBuilder ConfigureManyToManyAddressShopEntity(this ModelBuilder modelBuilder)
		{
			/*modelBuilder.Entity<AddressEntity>().Property(b => b.Point).HasColumnType("sys.geography");*/
			modelBuilder.Entity<AddressShopEntity>(table =>
			{
				table.HasKey(sc => new { sc.ShopId, sc.AddressId });

				table.HasOne(sc => sc.Address)
					.WithMany(s => s.AddressesShops)
					.HasForeignKey(sc => sc.AddressId);

				table.HasOne(sc => sc.Shop)
					.WithMany(s => s.AddressesShops)
					.HasForeignKey(sc => sc.ShopId);
			});


			return modelBuilder;
		}
	}
}
