using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sameposty.DataAccess.Migrations;

/// <inheritdoc />
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
public partial class PostHasNew : Migration
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "PublishDate",
            table: "Posts",
            newName: "ShedulePublishDate");

        migrationBuilder.AddColumn<bool>(
            name: "IsPublished",
            table: "Posts",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<string>(
            name: "PlatformPostId",
            table: "Posts",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<DateTime>(
            name: "PublishedDate",
            table: "Posts",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsPublished",
            table: "Posts");

        migrationBuilder.DropColumn(
            name: "PlatformPostId",
            table: "Posts");

        migrationBuilder.DropColumn(
            name: "PublishedDate",
            table: "Posts");

        migrationBuilder.RenameColumn(
            name: "ShedulePublishDate",
            table: "Posts",
            newName: "PublishDate");
    }
}
