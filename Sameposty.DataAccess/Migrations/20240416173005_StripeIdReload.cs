using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sameposty.DataAccess.Migrations;

/// <inheritdoc />
public partial class StripeIdReload : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CanceledAt",
            table: "Subscriptions");

        migrationBuilder.DropColumn(
            name: "IsCanceled",
            table: "Subscriptions");

        migrationBuilder.DropColumn(
            name: "StripeCusomerId",
            table: "Subscriptions");

        migrationBuilder.DropColumn(
            name: "StripePaymentCardId",
            table: "Subscriptions");

        migrationBuilder.RenameColumn(
            name: "AmountPaid",
            table: "Subscriptions",
            newName: "TotalAmountPaid");

        migrationBuilder.AddColumn<string>(
            name: "StripeCustomerId",
            table: "Users",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<double>(
            name: "LastAmountPaid",
            table: "Subscriptions",
            type: "float",
            nullable: false,
            defaultValue: 0.0);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "StripeCustomerId",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "LastAmountPaid",
            table: "Subscriptions");

        migrationBuilder.RenameColumn(
            name: "TotalAmountPaid",
            table: "Subscriptions",
            newName: "AmountPaid");

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
            name: "StripeCusomerId",
            table: "Subscriptions",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "StripePaymentCardId",
            table: "Subscriptions",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");
    }
}
