using Microsoft.EntityFrameworkCore.Migrations;

namespace FileSharingApp.Migrations
{
    public partial class addOriginalFileNAmeToUploads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OriginalFileName",
                table: "Uploads",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalFileName",
                table: "Uploads");
        }
    }
}
