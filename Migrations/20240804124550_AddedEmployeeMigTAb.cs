using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Migrations
{
    /// <inheritdoc />
    public partial class AddedEmployeeMigTAb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProject_EmployeesTable_ProjectEmployeeEmployeeId",
                table: "EmployeeProject");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProject_ProjectTable_E_id",
                table: "EmployeeProject");

            migrationBuilder.DropTable(
                name: "ClientProject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeProject",
                table: "EmployeeProject");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeProject_ProjectEmployeeEmployeeId",
                table: "EmployeeProject");

            migrationBuilder.DropColumn(
                name: "C_id",
                table: "ProjectTable");

            migrationBuilder.DropColumn(
                name: "E_id",
                table: "EmployeeProject");

            migrationBuilder.RenameColumn(
                name: "E_id",
                table: "ProjectTable",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "ProjectEmployeeEmployeeId",
                table: "EmployeeProject",
                newName: "Id");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "EmployeeProject",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "EmployeeProject",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeProject",
                table: "EmployeeProject",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTable_ClientId",
                table: "ProjectTable",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProject_EmployeeId",
                table: "EmployeeProject",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProject_ProjectId",
                table: "EmployeeProject",
                column: "ProjectId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTable_ClientTable_ClientId",
                table: "ProjectTable",
                column: "ClientId",
                principalTable: "ClientTable",
                principalColumn: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProject_EmployeesTable_EmployeeId",
                table: "EmployeeProject");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProject_ProjectTable_ProjectId",
                table: "EmployeeProject");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTable_ClientTable_ClientId",
                table: "ProjectTable");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTable_ClientId",
                table: "ProjectTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeProject",
                table: "EmployeeProject");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeProject_EmployeeId",
                table: "EmployeeProject");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeProject_ProjectId",
                table: "EmployeeProject");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "EmployeeProject");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "EmployeeProject");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "ProjectTable",
                newName: "E_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "EmployeeProject",
                newName: "ProjectEmployeeEmployeeId");

            migrationBuilder.AddColumn<Guid>(
                name: "C_id",
                table: "ProjectTable",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "E_id",
                table: "EmployeeProject",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeProject",
                table: "EmployeeProject",
                columns: new[] { "E_id", "ProjectEmployeeEmployeeId" });

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

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProject_ProjectEmployeeEmployeeId",
                table: "EmployeeProject",
                column: "ProjectEmployeeEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientProject_ProjectClientClientId",
                table: "ClientProject",
                column: "ProjectClientClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProject_EmployeesTable_ProjectEmployeeEmployeeId",
                table: "EmployeeProject",
                column: "ProjectEmployeeEmployeeId",
                principalTable: "EmployeesTable",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProject_ProjectTable_E_id",
                table: "EmployeeProject",
                column: "E_id",
                principalTable: "ProjectTable",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
