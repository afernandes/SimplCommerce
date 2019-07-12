using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SimplCommerce.WebHost.Migrations
{
    public partial class currency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DefaultCurrencyId",
                table: "Core_User",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Core_Currency",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    CurrencyCode = table.Column<string>(nullable: true),
                    Rate = table.Column<decimal>(nullable: false),
                    DisplayLocale = table.Column<string>(nullable: true),
                    CustomFormatting = table.Column<string>(nullable: true),
                    LimitedToStores = table.Column<bool>(nullable: false),
                    Published = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(nullable: false),
                    RoundingTypeId = table.Column<int>(nullable: false),
                    RoundingType = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_Currency", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Core_User_DefaultCurrencyId",
                table: "Core_User",
                column: "DefaultCurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Core_User_Core_Currency_DefaultCurrencyId",
                table: "Core_User",
                column: "DefaultCurrencyId",
                principalTable: "Core_Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Core_User_Core_Currency_DefaultCurrencyId",
                table: "Core_User");

            migrationBuilder.DropTable(
                name: "Core_Currency");

            migrationBuilder.DropIndex(
                name: "IX_Core_User_DefaultCurrencyId",
                table: "Core_User");

            migrationBuilder.DropColumn(
                name: "DefaultCurrencyId",
                table: "Core_User");
        }
    }
}
