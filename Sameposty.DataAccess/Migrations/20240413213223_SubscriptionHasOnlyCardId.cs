using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sameposty.DataAccess.Migrations;

/// <inheritdoc />
public partial class SubscriptionHasOnlyCardId : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CardLastFourDigits",
            table: "Subscriptions");

        migrationBuilder.DropColumn(
            name: "CardTokenId",
            table: "Subscriptions");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "CardLastFourDigits",
            table: "Subscriptions",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "CardTokenId",
            table: "Subscriptions",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");
    }
}
