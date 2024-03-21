using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sameposty.DataAccess.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Salt = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                NIP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                CompanyDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                IsVerified = table.Column<bool>(type: "bit", nullable: false),
                CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "BasicInformations",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Branch = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                ProductsAndServices = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Goals = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Assets = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                UserId = table.Column<int>(type: "int", nullable: false),
                CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BasicInformations", x => x.Id);
                table.ForeignKey(
                    name: "FK_BasicInformations_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "FacebookConnections",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                AccesToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PageId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                UserId = table.Column<int>(type: "int", nullable: false),
                CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FacebookConnections", x => x.Id);
                table.ForeignKey(
                    name: "FK_FacebookConnections_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "InstagramConnections",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                AccesToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PageId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                UserId = table.Column<int>(type: "int", nullable: false),
                CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_InstagramConnections", x => x.Id);
                table.ForeignKey(
                    name: "FK_InstagramConnections_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Posts",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                ImageUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                ShedulePublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                PlatformPostId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                IsPublished = table.Column<bool>(type: "bit", nullable: false),
                JobPublishId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                UserId = table.Column<int>(type: "int", nullable: false),
                CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Posts", x => x.Id);
                table.ForeignKey(
                    name: "FK_Posts_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Privileges",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CanGenerateInitialPosts = table.Column<bool>(type: "bit", nullable: false),
                CanGenerateImageAI = table.Column<bool>(type: "bit", nullable: false),
                CanEditImageAI = table.Column<bool>(type: "bit", nullable: false),
                CanGenerateTextAI = table.Column<bool>(type: "bit", nullable: false),
                UserId = table.Column<int>(type: "int", nullable: false),
                CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Privileges", x => x.Id);
                table.ForeignKey(
                    name: "FK_Privileges_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

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
                UserId = table.Column<int>(type: "int", nullable: false),
                PostId = table.Column<int>(type: "int", nullable: true),
                CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PublishResults", x => x.Id);
                table.ForeignKey(
                    name: "FK_PublishResults_Posts_PostId",
                    column: x => x.PostId,
                    principalTable: "Posts",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_BasicInformations_UserId",
            table: "BasicInformations",
            column: "UserId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_FacebookConnections_UserId",
            table: "FacebookConnections",
            column: "UserId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_InstagramConnections_UserId",
            table: "InstagramConnections",
            column: "UserId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Posts_UserId",
            table: "Posts",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_Privileges_UserId",
            table: "Privileges",
            column: "UserId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_PublishResults_PostId",
            table: "PublishResults",
            column: "PostId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "BasicInformations");

        migrationBuilder.DropTable(
            name: "FacebookConnections");

        migrationBuilder.DropTable(
            name: "InstagramConnections");

        migrationBuilder.DropTable(
            name: "Privileges");

        migrationBuilder.DropTable(
            name: "PublishResults");

        migrationBuilder.DropTable(
            name: "Posts");

        migrationBuilder.DropTable(
            name: "Users");
    }
}
