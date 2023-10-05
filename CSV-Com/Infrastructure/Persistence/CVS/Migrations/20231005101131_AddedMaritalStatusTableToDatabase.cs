using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.CVS.Migrations
{
    /// <inheritdoc />
    public partial class AddedMaritalStatusTableToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaritalStatus",
                table: "Clients",
                newName: "MaritalStatusId");

            migrationBuilder.CreateTable(
                name: "MaritalStatus",
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
                    table.PrimaryKey("PK_MaritalStatus", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_MaritalStatusId",
                table: "Clients",
                column: "MaritalStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_MaritalStatus_MaritalStatusId",
                table: "Clients",
                column: "MaritalStatusId",
                principalTable: "MaritalStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_MaritalStatus_MaritalStatusId",
                table: "Clients");

            migrationBuilder.DropTable(
                name: "MaritalStatus");

            migrationBuilder.DropIndex(
                name: "IX_Clients_MaritalStatusId",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "MaritalStatusId",
                table: "Clients",
                newName: "MaritalStatus");
        }
    }
}
