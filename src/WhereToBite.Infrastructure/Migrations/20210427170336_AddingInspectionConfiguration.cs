using Microsoft.EntityFrameworkCore.Migrations;

namespace WhereToBite.Infrastructure.Migrations
{
    public partial class AddingInspectionConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Establishments_EstablishmentStatus_EstablishmentStatusId",
                schema: "w2b",
                table: "Establishments");

            migrationBuilder.DropForeignKey(
                name: "FK_Infraction_InfractionAction_InfractionActionId",
                schema: "w2b",
                table: "Infraction");

            migrationBuilder.DropForeignKey(
                name: "FK_Infraction_Severity_SeverityId",
                schema: "w2b",
                table: "Infraction");

            migrationBuilder.DropForeignKey(
                name: "FK_Inspection_InspectionStatus_InspectionStatusId",
                schema: "w2b",
                table: "Inspection");

            migrationBuilder.DropIndex(
                name: "IX_Inspection_InspectionStatusId",
                schema: "w2b",
                table: "Inspection");

            migrationBuilder.DropIndex(
                name: "IX_Infraction_InfractionActionId",
                schema: "w2b",
                table: "Infraction");

            migrationBuilder.DropIndex(
                name: "IX_Infraction_SeverityId",
                schema: "w2b",
                table: "Infraction");

            migrationBuilder.DropIndex(
                name: "IX_Establishments_EstablishmentStatusId",
                schema: "w2b",
                table: "Establishments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Inspection_InspectionStatusId",
                schema: "w2b",
                table: "Inspection",
                column: "InspectionStatusId");

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
                name: "IX_Establishments_EstablishmentStatusId",
                schema: "w2b",
                table: "Establishments",
                column: "EstablishmentStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Establishments_EstablishmentStatus_EstablishmentStatusId",
                schema: "w2b",
                table: "Establishments",
                column: "EstablishmentStatusId",
                principalSchema: "w2b",
                principalTable: "EstablishmentStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Infraction_InfractionAction_InfractionActionId",
                schema: "w2b",
                table: "Infraction",
                column: "InfractionActionId",
                principalSchema: "w2b",
                principalTable: "InfractionAction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Infraction_Severity_SeverityId",
                schema: "w2b",
                table: "Infraction",
                column: "SeverityId",
                principalSchema: "w2b",
                principalTable: "Severity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inspection_InspectionStatus_InspectionStatusId",
                schema: "w2b",
                table: "Inspection",
                column: "InspectionStatusId",
                principalSchema: "w2b",
                principalTable: "InspectionStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
