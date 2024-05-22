using Financial.Cat.Domain.Models.Entities;
using Financial.Cat.Infrustructure.DB.Contexts.Abstract;
using Financial.Cat.Infrustructure.Extensions.DbExtenstions;
using Microsoft.EntityFrameworkCore;

namespace Financial.Cat.Infrustructure.DB.Contexts
{
	public class ApplicationContext : BaseDbContext
	{
		/// <summary>
		/// DB scheme
		/// </summary>
		public const string SchemaName = "Financial.Cat.Api";

		public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
		{
		}
		/// <summary>
		/// Calling all settings to model creating DB
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder
				.SetEntitiesAutoIncludes()
				.ConfigureOneToManyEntities()
				.ConfigureOneToOneEntities()
				.ConfigureManyToManyEntities()
				.DisableCascadeDeletes();

			modelBuilder.SetEnumToStringConversion(32);

			base.OnModelCreating(modelBuilder);
		}


		public DbSet<PurchaseEntity> Purchases { get; set; }
		public DbSet<UserEntity> Users { get; set; }
		public DbSet<ItemEntity> Items { get; set; }
		public DbSet<AddressEntity> Addresses { get; set; }
		public DbSet<AddressShopEntity> AddressShops { get; set; }
		public DbSet<ShopEntity> Shops { get; set; }
		public DbSet<ItemNomenclatureEntity> ItemNomenclatures { get; set; }
		public DbSet<CategoryEntity> Categories { get; set; }
		public DbSet<AuthOperationEntity> AuthOperations { get; set; }
		public DbSet<OtpEntity> Otps { get; set; }
		public DbSet<SettingLimitEntity> SettingLimits { get; set; }
	}
}