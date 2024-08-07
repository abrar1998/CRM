using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Migrations
{
    /// <inheritdoc />
    public partial class FixedIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTable_ClientTable_Client_ProjectId",
                table: "ProjectTable");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTable_EmployeesTable_Emp_ProjectId",
                table: "ProjectTable");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTable_Client_ProjectId",
                table: "ProjectTable");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTable_Emp_ProjectId",
                table: "ProjectTable");

            migrationBuilder.DropColumn(
                name: "Emp_ProjectId",
                table: "EmployeesTable");

            migrationBuilder.DropColumn(
                name: "Client_ProjectId",
                table: "ClientTable");

            migrationBuilder.RenameColumn(
                name: "Emp_ProjectId",
                table: "ProjectTable",
                newName: "E_id");

            migrationBuilder.RenameColumn(
                name: "Client_ProjectId",
                table: "ProjectTable",
                newName: "C_id");

            migrationBuilder.CreateTable(
                name: "ClientProject",
                columns: table => new
                {
                    C_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectClientClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProject", x => new { x.C_id, x.ProjectClientClientId });
                    table.ForeignKey(
                        name: "FK_ClientProject_ClientTable_ProjectClientClientId",
                        column: x => x.ProjectClientClientId,
                        principalTable: "ClientTable",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientProject_ProjectTable_C_id",
                        column: x => x.C_id,
                        principalTable: "ProjectTable",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeProject",
                columns: table => new
                {
                    E_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectEmployeeEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeProject", x => new { x.E_id, x.ProjectEmployeeEmployeeId });
                    table.ForeignKey(
                        name: "FK_EmployeeProject_EmployeesTable_ProjectEmployeeEmployeeId",
                        column: x => x.ProjectEmployeeEmployeeId,
                        principalTable: "EmployeesTable",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeProject_ProjectTable_E_id",
                        column: x => x.E_id,
                        principalTable: "ProjectTable",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientProject_ProjectClientClientId",
                table: "ClientProject",
                column: "ProjectClientClientId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProject_ProjectEmployeeEmployeeId",
                table: "EmployeeProject",
                column: "ProjectEmployeeEmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientProject");

            migrationBuilder.DropTable(
                name: "EmployeeProject");

            migrationBuilder.RenameColumn(
                name: "E_id",
                table: "ProjectTable",
                newName: "Emp_ProjectId");

            migrationBuilder.RenameColumn(
                name: "C_id",
                table: "ProjectTable",
                newName: "Client_ProjectId");

            migrationBuilder.AddColumn<Guid>(
                name: "Emp_ProjectId",
                table: "EmployeesTable",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Client_ProjectId",
                table: "ClientTable",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTable_Client_ProjectId",
                table: "ProjectTable",
                column: "Client_ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTable_Emp_ProjectId",
                table: "ProjectTable",
                column: "Emp_ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTable_ClientTable_Client_ProjectId",
                table: "ProjectTable",
                column: "Client_ProjectId",
                principalTable: "ClientTable",
                principalColumn: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTable_EmployeesTable_Emp_ProjectId",
                table: "ProjectTable",
                column: "Emp_ProjectId",
                principalTable: "EmployeesTable",
                principalColumn: "EmployeeId");
        }
    }
}
