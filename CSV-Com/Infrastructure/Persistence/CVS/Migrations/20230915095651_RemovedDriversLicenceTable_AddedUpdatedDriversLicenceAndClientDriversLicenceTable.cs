using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.CVS.Migrations
{
    /// <inheritdoc />
    public partial class RemovedDriversLicenceTable_AddedUpdatedDriversLicenceAndClientDriversLicenceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriversLicence_Clients_ClientId",
                table: "DriversLicence");

            migrationBuilder.DropTable(
                name: "ClientDriverLicence");

            migrationBuilder.DropTable(
                name: "DriverLicence");

            migrationBuilder.DropIndex(
                name: "IX_DriversLicence_ClientId",
                table: "DriversLicence");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "DriversLicence");

            migrationBuilder.DropColumn(
                name: "DriversLicenceCode",
                table: "DriversLicence");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "DriversLicence",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Omschrijving",
                table: "DriversLicence",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ClientDriversLicence",
                columns: table => new
                {
                    ClientsId = table.Column<int>(type: "int", nullable: false),
                    DriversLicencesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientDriversLicence", x => new { x.ClientsId, x.DriversLicencesId });
                    table.ForeignKey(
                        name: "FK_ClientDriversLicence_Clients_ClientsId",
                        column: x => x.ClientsId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientDriversLicence_DriversLicence_DriversLicencesId",
                        column: x => x.DriversLicencesId,
                        principalTable: "DriversLicence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ClientDriversLicence_DriversLicencesId",
                table: "ClientDriversLicence",
                column: "DriversLicencesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientDriversLicence");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "DriversLicence");

            migrationBuilder.DropColumn(
                name: "Omschrijving",
                table: "DriversLicence");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "DriversLicence",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DriversLicenceCode",
                table: "DriversLicence",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DriverLicence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Omschrijving = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverLicence", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ClientDriverLicence",
                columns: table => new
                {
                    ClientsId = table.Column<int>(type: "int", nullable: false),
                    DriverLicencesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientDriverLicence", x => new { x.ClientsId, x.DriverLicencesId });
                    table.ForeignKey(
                        name: "FK_ClientDriverLicence_Clients_ClientsId",
                        column: x => x.ClientsId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientDriverLicence_DriverLicence_DriverLicencesId",
                        column: x => x.DriverLicencesId,
                        principalTable: "DriverLicence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DriversLicence_ClientId",
                table: "DriversLicence",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientDriverLicence_DriverLicencesId",
                table: "ClientDriverLicence",
                column: "DriverLicencesId");

            migrationBuilder.AddForeignKey(
                name: "FK_DriversLicence_Clients_ClientId",
                table: "DriversLicence",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
