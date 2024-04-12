using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sameposty.DataAccess.Migrations;

/// <inheritdoc />
public partial class SubscriptionHasStripeFields : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CardTokenId",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "StripeCusomerId",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "OrderHasInvoice",
            table: "Subscriptions");

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

        migrationBuilder.AddColumn<string>(
            name: "StipeSubscriptionId",
            table: "Subscriptions",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "StripeCusomerId",
            table: "Subscriptions",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CardLastFourDigits",
            table: "Subscriptions");

        migrationBuilder.DropColumn(
            name: "CardTokenId",
            table: "Subscriptions");

        migrationBuilder.DropColumn(
            name: "StipeSubscriptionId",
            table: "Subscriptions");

        migrationBuilder.DropColumn(
            name: "StripeCusomerId",
            table: "Subscriptions");

        migrationBuilder.AddColumn<string>(
            name: "CardTokenId",
            table: "Users",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "StripeCusomerId",
            table: "Users",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<bool>(
            name: "OrderHasInvoice",
            table: "Subscriptions",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }
}
