using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentService.Migrations
{
    public partial class telegramerr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sendedmail",
                table: "UserTask");

            migrationBuilder.DropColumn(
                name: "sendedtelegram",
                table: "UserTask");

            migrationBuilder.AlterColumn<string>(
                name: "TextFinish",
                table: "UserTask",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Fiokey",
                table: "UserTask",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FioFinish",
                table: "UserTask",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Fio",
                table: "UserTask",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "errors",
                table: "UserTask",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fio",
                table: "UserTask");

            migrationBuilder.DropColumn(
                name: "errors",
                table: "UserTask");

            migrationBuilder.AlterColumn<string>(
                name: "TextFinish",
                table: "UserTask",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Fiokey",
                table: "UserTask",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FioFinish",
                table: "UserTask",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "sendedmail",
                table: "UserTask",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "sendedtelegram",
                table: "UserTask",
                type: "boolean",
                nullable: true);
        }
    }
}
