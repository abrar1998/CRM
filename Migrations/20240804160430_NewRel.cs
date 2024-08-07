using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Migrations
{
    /// <inheritdoc />
    public partial class NewRel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTable_ClientTable_ClientId",
                table: "ProjectTable");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "ProjectTable",
                newName: "ProjectManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTable_ClientId",
                table: "ProjectTable",
                newName: "IX_ProjectTable_ProjectManagerId");

            migrationBuilder.AddColumn<Guid>(
                name: "C_id",
                table: "ProjectTable",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClientProject",
                columns: table => new
                {
                    ProjectClientClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectsProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProject", x => new { x.ProjectClientClientId, x.ProjectsProjectId });
                    table.ForeignKey(
                        name: "FK_ClientProject_ClientTable_ProjectClientClientId",
                        column: x => x.ProjectClientClientId,
                        principalTable: "ClientTable",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientProject_ProjectTable_ProjectsProjectId",
                        column: x => x.ProjectsProjectId,
                        principalTable: "ProjectTable",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientProject_ProjectsProjectId",
                table: "ClientProject",
                column: "ProjectsProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTable_EmployeesTable_ProjectManagerId",
                table: "ProjectTable",
                column: "ProjectManagerId",
                principalTable: "EmployeesTable",
                principalColumn: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTable_EmployeesTable_ProjectManagerId",
                table: "ProjectTable");

            migrationBuilder.DropTable(
                name: "ClientProject");

            migrationBuilder.DropColumn(
                name: "C_id",
                table: "ProjectTable");

            migrationBuilder.RenameColumn(
                name: "ProjectManagerId",
                table: "ProjectTable",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTable_ProjectManagerId",
                table: "ProjectTable",
                newName: "IX_ProjectTable_ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTable_ClientTable_ClientId",
                table: "ProjectTable",
                column: "ClientId",
                principalTable: "ClientTable",
                principalColumn: "ClientId");
        }
    }
}
