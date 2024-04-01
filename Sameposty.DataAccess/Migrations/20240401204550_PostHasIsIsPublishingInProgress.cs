using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sameposty.DataAccess.Migrations;

/// <inheritdoc />
public partial class PostHasIsIsPublishingInProgress : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsPublishingInProgress",
            table: "Posts",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsPublishingInProgress",
            table: "Posts");
    }
}
