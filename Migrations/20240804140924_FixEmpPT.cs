using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Migrations
{
    /// <inheritdoc />
    public partial class FixEmpPT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProject_EmployeesTable_EmployeeId",
                table: "EmployeeProject");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProject_ProjectTable_ProjectId",
                table: "EmployeeProject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeProject",
                table: "EmployeeProject");

            migrationBuilder.RenameTable(
                name: "EmployeeProject",
                newName: "EmployeeProjectsTable");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeProject_ProjectId",
                table: "EmployeeProjectsTable",
                newName: "IX_EmployeeProjectsTable_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeProject_EmployeeId",
                table: "EmployeeProjectsTable",
                newName: "IX_EmployeeProjectsTable_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeProjectsTable",
                table: "EmployeeProjectsTable",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProjectsTable_EmployeesTable_EmployeeId",
                table: "EmployeeProjectsTable",
                column: "EmployeeId",
                principalTable: "EmployeesTable",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProjectsTable_ProjectTable_ProjectId",
                table: "EmployeeProjectsTable",
                column: "ProjectId",
                principalTable: "ProjectTable",
                principalColumn: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProjectsTable_EmployeesTable_EmployeeId",
                table: "EmployeeProjectsTable");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProjectsTable_ProjectTable_ProjectId",
                table: "EmployeeProjectsTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeProjectsTable",
                table: "EmployeeProjectsTable");

            migrationBuilder.RenameTable(
                name: "EmployeeProjectsTable",
                newName: "EmployeeProject");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeProjectsTable_ProjectId",
                table: "EmployeeProject",
                newName: "IX_EmployeeProject_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeProjectsTable_EmployeeId",
                table: "EmployeeProject",
                newName: "IX_EmployeeProject_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeProject",
                table: "EmployeeProject",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProject_EmployeesTable_EmployeeId",
                table: "EmployeeProject",
                column: "EmployeeId",
                principalTable: "EmployeesTable",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProject_ProjectTable_ProjectId",
                table: "EmployeeProject",
                column: "ProjectId",
                principalTable: "ProjectTable",
                principalColumn: "ProjectId");
        }
    }
}
