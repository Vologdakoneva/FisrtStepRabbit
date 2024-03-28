using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PacientService.Migrations
{
    public partial class telegramm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Person",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "telegram",
                table: "Person",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "usemail",
                table: "Person",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "usetelegram",
                table: "Person",
                type: "boolean",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "telegram",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "usemail",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "usetelegram",
                table: "Person");
        }
    }
}
