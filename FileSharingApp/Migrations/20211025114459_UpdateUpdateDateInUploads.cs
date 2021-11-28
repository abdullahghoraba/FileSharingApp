using Microsoft.EntityFrameworkCore.Migrations;

namespace FileSharingApp.Migrations
{
    public partial class UpdateUpdateDateInUploads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UploadTime",
                table: "Uploads",
                newName: "UploadDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UploadDate",
                table: "Uploads",
                newName: "UploadTime");
        }
    }
}
