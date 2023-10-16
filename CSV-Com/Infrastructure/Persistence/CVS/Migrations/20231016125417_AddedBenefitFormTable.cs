using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.CVS.Migrations
{
    /// <inheritdoc />
    public partial class AddedBenefitFormTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BenefitForm",
                table: "Clients",
                newName: "BenefitFormId");

            migrationBuilder.CreateTable(
                name: "BenefitForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenefitForm", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_BenefitFormId",
                table: "Clients",
                column: "BenefitFormId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_BenefitForm_BenefitFormId",
                table: "Clients",
                column: "BenefitFormId",
                principalTable: "BenefitForm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_BenefitForm_BenefitFormId",
                table: "Clients");

            migrationBuilder.DropTable(
                name: "BenefitForm");

            migrationBuilder.DropIndex(
                name: "IX_Clients_BenefitFormId",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "BenefitFormId",
                table: "Clients",
                newName: "BenefitForm");
        }
    }
}
