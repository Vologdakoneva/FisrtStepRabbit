using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PacientService.Migrations
{
    public partial class person : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    IDALL = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonLink = table.Column<Guid>(type: "uuid", nullable: true),
                    FamilyPerson = table.Column<string>(type: "text", nullable: false),
                    NamePerson = table.Column<string>(type: "text", nullable: false),
                    FathersPerson = table.Column<string>(type: "text", nullable: false),
                    birthDayPerson = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DataCreatePerson = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateChangePerson = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdPromedPerson = table.Column<int>(type: "integer", nullable: false),
                    Sex_idPerson = table.Column<int>(type: "integer", nullable: false),
                    Sex_Person = table.Column<string>(type: "text", nullable: false),
                    SnilsPerson = table.Column<string>(type: "text", nullable: false),
                    PhonePerson = table.Column<string>(type: "text", nullable: false),
                    SocStatus_id_Person = table.Column<int>(type: "integer", nullable: false),
                    SocStatus_Person = table.Column<string>(type: "text", nullable: false),
                    Inn_Person = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.IDALL);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
