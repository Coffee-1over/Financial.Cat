﻿// <auto-generated />
using System;
using Financial.Cat.Infrustructure.DB.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;

#nullable disable

namespace Financial.Cat.Infrastructure.Db.Contexts.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20240520204244_Migration_PurchaseTime")]
    partial class Migration_PurchaseTime
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Financial.Cat.Domain.Models.Entities.AddressEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Point>("Point")
                        .IsRequired()
                        .HasColumnType("geography");

                    b.Property<string>("Street1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Zip")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Financial.Cat.Domain.Models.Entities.AddressShopEntity", b =>
                {
                    b.Property<long>("ShopId")
                        .HasColumnType("bigint");

                    b.Property<long>("AddressId")
                        .HasColumnType("bigint");

                    b.HasKey("ShopId", "AddressId");

                    b.HasIndex("AddressId");

                    b.ToTable("AddressShops");
                });

            modelBuilder.Entity("Financial.Cat.Domain.Models.Entities.AuthOperationEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("AuthType")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("OperationStatus")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AuthOperations");
                });

            modelBuilder.Entity("Financial.Cat.Domain.Models.Entities.CategoryEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ParentCategoryId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Financial.Cat.Domain.Models.Entities.ItemEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("ItemNomenclatureId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("PurchaseId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ItemNomenclatureId");

                    b.HasIndex("PurchaseId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Financial.Cat.Domain.Models.Entities.ItemNomenclatureEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsHidden")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("ItemNomenclatures");
                });

            modelBuilder.Entity("Financial.Cat.Domain.Models.Entities.OtpEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Expired")
                        .HasColumnType("datetime2");

                    b.Property<string>("HashCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Otps");
                });

            modelBuilder.Entity("Financial.Cat.Domain.Models.Entities.PurchaseEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("AddressId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("PurchaseTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("UserId");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("Financial.Cat.Domain.Models.Entities.ShopEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Shops");
                });

            modelBuilder.Entity("Financial.Cat.Domain.Models.Entities.UserEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailVerified")
                        .HasColumnType("bit");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("TwoFactorKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Financial.Cat.Domain.Models.Entities.AddressShopEntity", b =>
                {
                    b.HasOne("Financial.Cat.Domain.Models.Entities.AddressEntity", "Address")
                        .WithMany("AddressesShops")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Financial.Cat.Domain.Models.Entities.ShopEntity", "Shop")
                        .WithMany("AddressesShops")
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("Shop");
                });

            modelBuilder.Entity("Financial.Cat.Domain.Models.Entities.AuthOperationEntity", b =>
                {
                    b.HasOne("Financial.Cat.Domain.Models.Entities.UserEntity", "User")
                        .WithMany("AuthOperations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Financial.Cat.Domain.Models.Entities.ItemEntity", b =>
                {
                    b.HasOne("Financial.Cat.Domain.Models.Entities.ItemNomenclatureEntity", "ItemNomenclature")
                        .WithMany("Items")
                        .HasForeignKey("ItemNomenclatureId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Financial.Cat.Domain.Models.Entities.PurchaseEntity", "Purchase")
                        .WithMany("Items")
                        .HasForeignKey("PurchaseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ItemNomenclature");

                    b.Navigation("Purchase");
                });

            modelBuilder.Entity("Financial.Cat.Domain.Models.Entities.ItemNomenclatureEntity", b =>
                {
                    b.HasOne("Financial.Cat.Domain.Models.Entities.CategoryEntity", "Category")
                        .WithMany("ItemNomenclatures")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Financial.Cat.Domain.Models.Entities.UserEntity", "User")
                        .WithMany("ItemNomenclatures")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Financial.Cat.Domain.Models.Entities.OtpEntity", b =>
                {
                    b.HasOne("Financial.Cat.Domain.Models.Entities.UserEntity", "User")
                        .WithMany("Otps")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Financial.Cat.Domain.Models.Entities.PurchaseEntity", b =>
                {
                    b.HasOne("Financial.Cat.Domain.Models.Entities.AddressEntity", "Address")
                        .WithMany("Purchases")
                        .HasForeignKey("AddressId");

                    b.HasOne("Financial.Cat.Domain.Models.Entities.UserEntity", "User")
                        .WithMany("Purchases")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Financial.Cat.Domain.Models.Entities.AddressEntity", b =>
                {
                    b.Navigation("AddressesShops");

                    b.Navigation("Purchases");
                });

            modelBuilder.Entity("Financial.Cat.Domain.Models.Entities.CategoryEntity", b =>
                {
                    b.Navigation("ItemNomenclatures");
                });

            modelBuilder.Entity("Financial.Cat.Domain.Models.Entities.ItemNomenclatureEntity", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Financial.Cat.Domain.Models.Entities.PurchaseEntity", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Financial.Cat.Domain.Models.Entities.ShopEntity", b =>
                {
                    b.Navigation("AddressesShops");
                });

            modelBuilder.Entity("Financial.Cat.Domain.Models.Entities.UserEntity", b =>
                {
                    b.Navigation("AuthOperations");

                    b.Navigation("ItemNomenclatures");

                    b.Navigation("Otps");

                    b.Navigation("Purchases");
                });
#pragma warning restore 612, 618
        }
    }
}
