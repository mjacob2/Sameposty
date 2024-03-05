using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sameposty.DataAccess.Migrations;

/// <inheritdoc />
public partial class initial2 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_BasicInformation_Users_UserId",
            table: "BasicInformation");

        migrationBuilder.DropPrimaryKey(
            name: "PK_BasicInformation",
            table: "BasicInformation");

        migrationBuilder.RenameTable(
            name: "BasicInformation",
            newName: "BasicInformations");

        migrationBuilder.RenameIndex(
            name: "IX_BasicInformation_UserId",
            table: "BasicInformations",
            newName: "IX_BasicInformations_UserId");

        migrationBuilder.AddPrimaryKey(
            name: "PK_BasicInformations",
            table: "BasicInformations",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_BasicInformations_Users_UserId",
            table: "BasicInformations",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_BasicInformations_Users_UserId",
            table: "BasicInformations");

        migrationBuilder.DropPrimaryKey(
            name: "PK_BasicInformations",
            table: "BasicInformations");

        migrationBuilder.RenameTable(
            name: "BasicInformations",
            newName: "BasicInformation");

        migrationBuilder.RenameIndex(
            name: "IX_BasicInformations_UserId",
            table: "BasicInformation",
            newName: "IX_BasicInformation_UserId");

        migrationBuilder.AddPrimaryKey(
            name: "PK_BasicInformation",
            table: "BasicInformation",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_BasicInformation_Users_UserId",
            table: "BasicInformation",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
