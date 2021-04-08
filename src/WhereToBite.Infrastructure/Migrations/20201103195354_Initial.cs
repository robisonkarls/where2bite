using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WhereToBite.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "w2b");

            migrationBuilder.CreateTable(
                name: "EstablishmentStatus",
                schema: "w2b",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstablishmentStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InfractionAction",
                schema: "w2b",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfractionAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InspectionStatus",
                schema: "w2b",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Severity",
                schema: "w2b",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Severity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Establishments",
                schema: "w2b",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DineSafeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    Longitude = table.Column<string>(nullable: false),
                    Latitude = table.Column<string>(nullable: false),
                    EstablishmentStatusId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Establishments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Establishments_EstablishmentStatus_EstablishmentStatusId",
                        column: x => x.EstablishmentStatusId,
                        principalSchema: "w2b",
                        principalTable: "EstablishmentStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inspection",
                schema: "w2b",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InspectionStatusId = table.Column<int>(nullable: false),
                    EstablishmentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inspection_Establishments_EstablishmentId",
                        column: x => x.EstablishmentId,
                        principalSchema: "w2b",
                        principalTable: "Establishments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inspection_InspectionStatus_InspectionStatusId",
                        column: x => x.InspectionStatusId,
                        principalSchema: "w2b",
                        principalTable: "InspectionStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Infraction",
                schema: "w2b",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SeverityId = table.Column<int>(nullable: false),
                    InfractionActionId = table.Column<int>(nullable: false),
                    CourtOutcome = table.Column<string>(nullable: true),
                    AmountFined = table.Column<decimal>(nullable: false),
                    InspectionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infraction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Infraction_Inspection_InspectionId",
                        column: x => x.InspectionId,
                        principalSchema: "w2b",
                        principalTable: "Inspection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Infraction_InfractionAction_InfractionActionId",
                        column: x => x.InfractionActionId,
                        principalSchema: "w2b",
                        principalTable: "InfractionAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Infraction_Severity_SeverityId",
                        column: x => x.SeverityId,
                        principalSchema: "w2b",
                        principalTable: "Severity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Establishments_DineSafeId",
                schema: "w2b",
                table: "Establishments",
                column: "DineSafeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Establishments_EstablishmentStatusId",
                schema: "w2b",
                table: "Establishments",
                column: "EstablishmentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Infraction_InspectionId",
                schema: "w2b",
                table: "Infraction",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Infraction_InfractionActionId",
                schema: "w2b",
                table: "Infraction",
                column: "InfractionActionId");

            migrationBuilder.CreateIndex(
                name: "IX_Infraction_SeverityId",
                schema: "w2b",
                table: "Infraction",
                column: "SeverityId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_EstablishmentId",
                schema: "w2b",
                table: "Inspection",
                column: "EstablishmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspection_InspectionStatusId",
                schema: "w2b",
                table: "Inspection",
                column: "InspectionStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Infraction",
                schema: "w2b");

            migrationBuilder.DropTable(
                name: "Inspection",
                schema: "w2b");

            migrationBuilder.DropTable(
                name: "InfractionAction",
                schema: "w2b");

            migrationBuilder.DropTable(
                name: "Severity",
                schema: "w2b");

            migrationBuilder.DropTable(
                name: "Establishments",
                schema: "w2b");

            migrationBuilder.DropTable(
                name: "InspectionStatus",
                schema: "w2b");

            migrationBuilder.DropTable(
                name: "EstablishmentStatus",
                schema: "w2b");
        }
    }
}
