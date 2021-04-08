using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace WhereToBite.Infrastructure.Migrations
{
    public partial class ChangingToGeography : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "w2b",
                table: "Establishments");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "w2b",
                table: "Establishments");

            migrationBuilder.AlterColumn<Point>(
                name: "Location",
                schema: "w2b",
                table: "Establishments",
                type: "geography (point)",
                nullable: true,
                oldClrType: typeof(Point),
                oldType: "geometry (point)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Point>(
                name: "Location",
                schema: "w2b",
                table: "Establishments",
                type: "geometry (point)",
                nullable: true,
                oldClrType: typeof(Point),
                oldType: "geography (point)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                schema: "w2b",
                table: "Establishments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                schema: "w2b",
                table: "Establishments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
