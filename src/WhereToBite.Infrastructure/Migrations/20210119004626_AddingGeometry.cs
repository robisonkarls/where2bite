using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace WhereToBite.Infrastructure.Migrations
{
    public partial class AddingGeometry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.AddColumn<Point>(
                name: "Location",
                schema: "w2b",
                table: "Establishments",
                type: "geometry (point)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                schema: "w2b",
                table: "Establishments");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:postgis", ",,");
        }
    }
}
