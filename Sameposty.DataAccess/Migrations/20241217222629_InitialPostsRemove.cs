using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sameposty.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostsRemove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanGenerateInitialPosts",
                table: "Privileges");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanGenerateInitialPosts",
                table: "Privileges",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
