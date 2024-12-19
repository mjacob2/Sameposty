using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sameposty.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Branch",
                table: "BasicInformations");

            migrationBuilder.RenameColumn(
                name: "TextTokensLimit",
                table: "Users",
                newName: "TextTokensLeft");

            migrationBuilder.RenameColumn(
                name: "ImageTokensLimit",
                table: "Users",
                newName: "ImageTokensLeft");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TextTokensLeft",
                table: "Users",
                newName: "TextTokensLimit");

            migrationBuilder.RenameColumn(
                name: "ImageTokensLeft",
                table: "Users",
                newName: "ImageTokensLimit");

            migrationBuilder.AddColumn<string>(
                name: "Branch",
                table: "BasicInformations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
