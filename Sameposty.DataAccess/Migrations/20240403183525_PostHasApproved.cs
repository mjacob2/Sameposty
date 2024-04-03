using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sameposty.DataAccess.Migrations;

/// <inheritdoc />
public partial class PostHasApproved : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsApproved",
            table: "Posts",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsApproved",
            table: "Posts");
    }
}
