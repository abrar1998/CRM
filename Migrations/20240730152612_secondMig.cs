using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Migrations
{
    /// <inheritdoc />
    public partial class secondMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "EmployeesTable",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "EmployeesTable",
                newName: "EmployeeDesignation");

            migrationBuilder.AddColumn<DateOnly>(
                name: "JoiningDate",
                table: "EmployeesTable",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "JoiningDate",
                table: "ClientTable",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "JoiningDate",
                table: "AdminTable",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JoiningDate",
                table: "EmployeesTable");

            migrationBuilder.DropColumn(
                name: "JoiningDate",
                table: "ClientTable");

            migrationBuilder.DropColumn(
                name: "JoiningDate",
                table: "AdminTable");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "EmployeesTable",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "EmployeeDesignation",
                table: "EmployeesTable",
                newName: "FirstName");
        }
    }
}
