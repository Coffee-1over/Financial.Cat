using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Financial.Cat.Infrastructure.Db.Contexts.Migrations
{
    /// <inheritdoc />
    public partial class Migration_PurchaseTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseTime",
                table: "Purchases",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseTime",
                table: "Purchases");
        }
    }
}
