using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PacientService.Migrations
{
    public partial class Addpers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "successfully",
                table: "Person",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Person_PersonLink",
                table: "Person",
                column: "PersonLink");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Person_PersonLink",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "successfully",
                table: "Person");
        }
    }
}
