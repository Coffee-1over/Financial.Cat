using Financial.Cat.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financial.Cat.Infrustructure.Extensions.DbExtenstions
{
	internal static class OneToManyExtensions
	{
		public static ModelBuilder ConfigureOneToManyEntities(this ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PurchaseEntity>(table =>
			{
				table.HasOne(x => x.User)
					.WithMany(x => x.Purchases)
					.HasForeignKey(x => x.UserId)
					.IsRequired();

				table.HasOne(x => x.Address)
					.WithMany(x => x.Purchases)
					.HasForeignKey(x => x.AddressId)
					.IsRequired(false);
			});


			modelBuilder.Entity<AuthOperationEntity>(table =>
			{
				table.HasOne(x => x.User)
					.WithMany(x => x.AuthOperations)
					.HasForeignKey(x => x.UserId)
					.IsRequired();
			});
			
			modelBuilder.Entity<SettingLimitEntity>(table =>
			{
				table.HasOne(x => x.User)
					.WithMany(x => x.SettingLimits)
					.HasForeignKey(x => x.UserId)
					.IsRequired();
			});

			modelBuilder.Entity<OtpEntity>(table =>
			{
				table.HasOne(x => x.User)
					.WithMany(x => x.Otps)
					.HasForeignKey(x => x.UserId)
					.IsRequired();
			});

			modelBuilder.Entity<ItemEntity>()
				.HasOne(x => x.Purchase)
				.WithMany(x => x.Items)
				.HasForeignKey(x => x.PurchaseId)
				.IsRequired();

			modelBuilder.Entity<ItemNomenclatureEntity>(table =>
			{
				table.HasOne(x => x.Category)
					.WithMany(x => x.ItemNomenclatures)
					.HasForeignKey(x => x.CategoryId)
					.IsRequired();

				table.HasOne(x => x.User)
					.WithMany(x => x.ItemNomenclatures)
					.HasForeignKey(x => x.UserId)
					.IsRequired();
			});
				


			return modelBuilder;
		}
	}
}
