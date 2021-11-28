using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FileSharingApp.Migrations
{
    public partial class addUploadsDateToUploads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UploadTime",
                table: "Uploads",
                type: "datetime2",
                nullable: false,
                defaultValueSql:"getDate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadTime",
                table: "Uploads");
        }
    }
}
