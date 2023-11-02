using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PacientService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ErrorPerson",
                columns: table => new
                {
                    IDALL = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonLink = table.Column<Guid>(type: "uuid", nullable: true),
                    ErrorSource = table.Column<string>(type: "text", nullable: true),
                    ErrorText = table.Column<string>(type: "text", nullable: true),
                    DataError = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorPerson", x => x.IDALL);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    IDALL = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonLink = table.Column<Guid>(type: "uuid", nullable: false),
                    FamilyPerson = table.Column<string>(type: "text", nullable: false),
                    NamePerson = table.Column<string>(type: "text", nullable: false),
                    FathersPerson = table.Column<string>(type: "text", nullable: false),
                    birthDayPerson = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DataCreatePerson = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateChangePerson = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdPromedPerson = table.Column<int>(type: "integer", nullable: true),
                    Sex_idPerson = table.Column<int>(type: "integer", nullable: true),
                    Sex_Person = table.Column<string>(type: "text", nullable: true),
                    SnilsPerson = table.Column<string>(type: "text", nullable: false),
                    PhonePerson = table.Column<string>(type: "text", nullable: true),
                    SocStatus_id_Person = table.Column<int>(type: "integer", nullable: true),
                    SocStatus_Person = table.Column<string>(type: "text", nullable: true),
                    Inn_Person = table.Column<string>(type: "text", nullable: true),
                    successfully = table.Column<bool>(type: "boolean", nullable: false),
                    viddoc = table.Column<int>(type: "integer", nullable: true),
                    SeriaDoc = table.Column<string>(type: "text", nullable: true),
                    NomDoc = table.Column<string>(type: "text", nullable: true),
                    DataDoc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    KemVidan = table.Column<string>(type: "text", nullable: true),
                    DataDeath = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    FomsName = table.Column<string>(type: "text", nullable: true),
                    FomsKod = table.Column<string>(type: "text", nullable: true),
                    PolisSeria = table.Column<string>(type: "text", nullable: true),
                    PolisNomer = table.Column<string>(type: "text", nullable: true),
                    IsVrach = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.IDALL);
                });

            migrationBuilder.CreateTable(
                name: "Setups",
                columns: table => new
                {
                    IDALL = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Namenastr = table.Column<string>(type: "text", nullable: false),
                    NamenRus = table.Column<string>(type: "text", nullable: false),
                    ValueString = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setups", x => x.IDALL);
                });

            migrationBuilder.InsertData(
                table: "Setups",
                columns: new[] { "IDALL", "NamenRus", "Namenastr", "ValueString" },
                values: new object[] { 1, "Url для сервиса пациенты", "URL_PACIENT", "http://localhost:39289/api/Pacient" });

            migrationBuilder.CreateIndex(
                name: "IX_Person_PersonLink",
                table: "Person",
                column: "PersonLink");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorPerson");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "Setups");
        }
    }
}
