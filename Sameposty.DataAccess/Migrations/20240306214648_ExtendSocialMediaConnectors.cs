using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sameposty.DataAccess.Migrations;

/// <inheritdoc />
public partial class ExtendSocialMediaConnectors : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "Name",
            table: "SocialMediaConnections",
            newName: "Platform");

        migrationBuilder.AddColumn<string>(
            name: "AccesToken",
            table: "SocialMediaConnections",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "PageId",
            table: "SocialMediaConnections",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "PageName",
            table: "SocialMediaConnections",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "AccesToken",
            table: "SocialMediaConnections");

        migrationBuilder.DropColumn(
            name: "PageId",
            table: "SocialMediaConnections");

        migrationBuilder.DropColumn(
            name: "PageName",
            table: "SocialMediaConnections");

        migrationBuilder.RenameColumn(
            name: "Platform",
            table: "SocialMediaConnections",
            newName: "Name");
    }
}
