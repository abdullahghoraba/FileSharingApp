using Microsoft.EntityFrameworkCore.Migrations;

namespace FileSharingApp.Migrations
{
    public partial class updateUploadsfieldToDownloadCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UploadCount",
                table: "Uploads",
                newName: "DownloadCount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DownloadCount",
                table: "Uploads",
                newName: "UploadCount");
        }
    }
}
