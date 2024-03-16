using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sameposty.DataAccess.Migrations;

/// <inheritdoc />
public partial class IntroducePublishResult : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "PublishResults",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Platform = table.Column<string>(type: "nvarchar(max)", nullable: false),
                IsPublishedSuccess = table.Column<bool>(type: "bit", nullable: false),
                PublishedPostId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Error = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PublishResults", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "PublishResults");
    }
}
