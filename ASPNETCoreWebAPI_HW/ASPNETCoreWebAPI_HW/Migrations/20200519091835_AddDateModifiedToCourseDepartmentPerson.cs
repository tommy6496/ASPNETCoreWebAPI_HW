using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPNETCoreWebAPI_HW.Migrations
{
    public partial class AddDateModifiedToCourseDepartmentPerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Person",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Department",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Course",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Course");
        }
    }
}
