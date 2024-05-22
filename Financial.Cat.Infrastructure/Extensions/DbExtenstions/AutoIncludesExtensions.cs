using Financial.Cat.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financial.Cat.Infrustructure.Extensions.DbExtenstions
{
	internal static class AutoIncludesExtensions
	{
		public static ModelBuilder SetEntitiesAutoIncludes(this ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PurchaseEntity>().Navigation(x => x.Address).AutoInclude();
			modelBuilder.Entity<PurchaseEntity>().Navigation(x => x.Items).AutoInclude();

			modelBuilder.Entity<AddressEntity>().Navigation(x => x.AddressesShops).AutoInclude();

			modelBuilder.Entity<ItemEntity>().Navigation(x => x.ItemNomenclature).AutoInclude();

			modelBuilder.Entity<ItemNomenclatureEntity>().Navigation(x => x.Category).AutoInclude();

			modelBuilder.Entity<AuthOperationEntity>().Navigation(x => x.User).AutoInclude();

			return modelBuilder;
		}
	}
}
