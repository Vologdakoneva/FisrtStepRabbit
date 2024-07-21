using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobileApp.Migrations
{
    public partial class _21073 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isWorker",
                table: "UsersApp",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isWorker",
                table: "UsersApp");
        }
    }
}
