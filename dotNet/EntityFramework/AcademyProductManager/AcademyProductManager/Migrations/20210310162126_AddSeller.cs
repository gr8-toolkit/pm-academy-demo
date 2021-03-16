using AcademyProductManager.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AcademyProductManager.Migrations
{
    public partial class AddSeller : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Seller>(
                name: "Seller",
                table: "Products",
                type: "jsonb",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Seller",
                table: "Products");
        }
    }
}
