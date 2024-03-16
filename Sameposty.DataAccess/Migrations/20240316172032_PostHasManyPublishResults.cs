using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sameposty.DataAccess.Migrations;

/// <inheritdoc />
public partial class PostHasManyPublishResults : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "UserId",
            table: "PublishResults",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateIndex(
            name: "IX_PublishResults_UserId",
            table: "PublishResults",
            column: "UserId");

        migrationBuilder.AddForeignKey(
            name: "FK_PublishResults_Posts_UserId",
            table: "PublishResults",
            column: "UserId",
            principalTable: "Posts",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_PublishResults_Posts_UserId",
            table: "PublishResults");

        migrationBuilder.DropIndex(
            name: "IX_PublishResults_UserId",
            table: "PublishResults");

        migrationBuilder.DropColumn(
            name: "UserId",
            table: "PublishResults");
    }
}
