using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sameposty.DataAccess.Migrations;

/// <inheritdoc />
public partial class Subscriptionhanges : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CompanyDescription",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "CustomerEmail",
            table: "Subscriptions");

        migrationBuilder.DropColumn(
            name: "OrderId",
            table: "Subscriptions");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "CompanyDescription",
            table: "Users",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "CustomerEmail",
            table: "Subscriptions",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<int>(
            name: "OrderId",
            table: "Subscriptions",
            type: "int",
            nullable: false,
            defaultValue: 0);
    }
}
