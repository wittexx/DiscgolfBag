using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscGolfBag.Api.Migrations
{
    public partial class AddDiscDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Discs",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Discs");
        }
    }
}
