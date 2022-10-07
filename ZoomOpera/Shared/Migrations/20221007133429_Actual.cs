using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZoomOpera.Shared.Migrations
{
    public partial class Actual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "OperaImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "OperaImages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "OperaImages");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "OperaImages");
        }
    }
}
