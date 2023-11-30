using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.CVS.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDiagnosistable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diagnosis_Clients_ClientId",
                table: "Diagnosis");

            migrationBuilder.DropIndex(
                name: "IX_Diagnosis_ClientId",
                table: "Diagnosis");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Diagnosis");

            migrationBuilder.CreateTable(
                name: "ClientDiagnosis",
                columns: table => new
                {
                    ClientsId = table.Column<int>(type: "int", nullable: false),
                    DiagnosesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientDiagnosis", x => new { x.ClientsId, x.DiagnosesId });
                    table.ForeignKey(
                        name: "FK_ClientDiagnosis_Clients_ClientsId",
                        column: x => x.ClientsId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientDiagnosis_Diagnosis_DiagnosesId",
                        column: x => x.DiagnosesId,
                        principalTable: "Diagnosis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ClientDiagnosis_DiagnosesId",
                table: "ClientDiagnosis",
                column: "DiagnosesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientDiagnosis");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Diagnosis",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Diagnosis_ClientId",
                table: "Diagnosis",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnosis_Clients_ClientId",
                table: "Diagnosis",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
