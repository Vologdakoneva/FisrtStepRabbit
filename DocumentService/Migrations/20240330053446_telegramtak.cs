using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DocumentService.Migrations
{
    public partial class telegramtak : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTask",
                columns: table => new
                {
                    IDALL = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PriorityTask = table.Column<string>(type: "text", nullable: false),
                    DataTask = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FioExec = table.Column<string>(type: "text", nullable: false),
                    successfully = table.Column<bool>(type: "boolean", nullable: false),
                    FioFinish = table.Column<string>(type: "text", nullable: false),
                    TextFinish = table.Column<string>(type: "text", nullable: false),
                    DocLink = table.Column<Guid>(type: "uuid", nullable: false),
                    TextTask = table.Column<string>(type: "text", nullable: false),
                    ownertask = table.Column<string>(type: "text", nullable: false),
                    Fiokey = table.Column<string>(type: "text", nullable: false),
                    IdFio = table.Column<long>(type: "bigint", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    usemail = table.Column<bool>(type: "boolean", nullable: true),
                    sendedmail = table.Column<bool>(type: "boolean", nullable: true),
                    telegram = table.Column<string>(type: "text", nullable: true),
                    usetelegram = table.Column<bool>(type: "boolean", nullable: true),
                    sendedtelegram = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTask", x => x.IDALL);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTask_DocLink",
                table: "UserTask",
                column: "DocLink");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTask");
        }
    }
}
