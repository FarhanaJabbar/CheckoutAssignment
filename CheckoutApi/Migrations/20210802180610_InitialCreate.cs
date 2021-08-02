using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CheckoutApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SecurityCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CurrencyCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CardExpiryMonth = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CardExpiryYear = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CardHolderName = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    BillingAddressLine1 = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    BillingAddressLine2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BillingAddressCity = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BillingAddressCounty = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BillingAddressPostcode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BillingAddressCountry = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction");
        }
    }
}
