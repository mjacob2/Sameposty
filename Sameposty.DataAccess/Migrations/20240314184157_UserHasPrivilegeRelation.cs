using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sameposty.DataAccess.Migrations;

/// <inheritdoc />
public partial class UserHasPrivilegeRelation : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "UserId",
            table: "Privileges",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateIndex(
            name: "IX_Privileges_UserId",
            table: "Privileges",
            column: "UserId",
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_Privileges_Users_UserId",
            table: "Privileges",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Privileges_Users_UserId",
            table: "Privileges");

        migrationBuilder.DropIndex(
            name: "IX_Privileges_UserId",
            table: "Privileges");

        migrationBuilder.DropColumn(
            name: "UserId",
            table: "Privileges");
    }
}
