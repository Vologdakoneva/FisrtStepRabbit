using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PacientService.Migrations
{
    public partial class dt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Setups",
                columns: new[] { "IDALL", "NamenRus", "Namenastr", "ValueString" },
                values: new object[] { 1, "Url для сервиса пациенты", "URL_PACIENT", "http://localhost:39289/api/Pacient" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Setups",
                keyColumn: "IDALL",
                keyValue: 1);
        }
    }
}
