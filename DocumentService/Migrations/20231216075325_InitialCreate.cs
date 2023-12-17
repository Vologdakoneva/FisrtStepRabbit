using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DocumentService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocAnaliz",
                columns: table => new
                {
                    IDALL = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DocLink = table.Column<Guid>(type: "uuid", nullable: false),
                    NomDoc = table.Column<string>(type: "text", nullable: false),
                    Datadoc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Fio = table.Column<string>(type: "text", nullable: false),
                    Fiokey = table.Column<string>(type: "text", nullable: false),
                    IdFio = table.Column<int>(type: "integer", nullable: false),
                    FioDoctor = table.Column<string>(type: "text", nullable: false),
                    FioDoctorkey = table.Column<string>(type: "text", nullable: false),
                    IdDoctor = table.Column<int>(type: "integer", nullable: false),
                    Databiomaterial = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocAnaliz", x => x.IDALL);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocAnaliz_DocLink",
                table: "DocAnaliz",
                column: "DocLink");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocAnaliz");
        }
    }
}
