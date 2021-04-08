using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WhereToBite.Infrastructure.Migrations
{
    public partial class addingdatecolumntoinspection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                schema: "w2b",
                table: "Inspection",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "CourtOutcome",
                schema: "w2b",
                table: "Infraction",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                schema: "w2b",
                table: "Inspection");

            migrationBuilder.AlterColumn<string>(
                name: "CourtOutcome",
                schema: "w2b",
                table: "Infraction",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
