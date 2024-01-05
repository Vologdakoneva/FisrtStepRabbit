using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentService.Migrations
{
    public partial class errs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Errors",
                table: "DocAnaliz",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Items",
                table: "DocAnaliz",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Recomedation",
                table: "DocAnaliz",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "successfully",
                table: "DocAnaliz",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Errors",
                table: "DocAnaliz");

            migrationBuilder.DropColumn(
                name: "Items",
                table: "DocAnaliz");

            migrationBuilder.DropColumn(
                name: "Recomedation",
                table: "DocAnaliz");

            migrationBuilder.DropColumn(
                name: "successfully",
                table: "DocAnaliz");
        }
    }
}
