using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.CVS.Migrations
{
    /// <inheritdoc />
    public partial class MartialStatusRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_MaritalStatuses_MaritalStatusId",
                table: "Clients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaritalStatuses",
                table: "MaritalStatuses");

            migrationBuilder.RenameTable(
                name: "MaritalStatuses",
                newName: "MaritalStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaritalStatus",
                table: "MaritalStatus",
                column: "Id");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaritalStatus",
                table: "MaritalStatus");

            migrationBuilder.RenameTable(
                name: "MaritalStatus",
                newName: "MaritalStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaritalStatuses",
                table: "MaritalStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_MaritalStatuses_MaritalStatusId",
                table: "Clients",
                column: "MaritalStatusId",
                principalTable: "MaritalStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
