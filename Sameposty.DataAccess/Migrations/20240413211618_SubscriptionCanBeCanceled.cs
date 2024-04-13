using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sameposty.DataAccess.Migrations;

/// <inheritdoc />
public partial class SubscriptionCanBeCanceled : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "CanceledAt",
            table: "Subscriptions",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsCanceled",
            table: "Subscriptions",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<string>(
            name: "StripePaymentCardId",
            table: "Subscriptions",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CanceledAt",
            table: "Subscriptions");

        migrationBuilder.DropColumn(
            name: "IsCanceled",
            table: "Subscriptions");

        migrationBuilder.DropColumn(
            name: "StripePaymentCardId",
            table: "Subscriptions");
    }
}
