using Microsoft.EntityFrameworkCore.Migrations;

namespace FileSharingApp.Migrations
{
    public partial class UpdateContactIsClosedField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Contacts",
                newName: "IsClosed");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsClosed",
                table: "Contacts",
                newName: "Status");
        }
    }
}
