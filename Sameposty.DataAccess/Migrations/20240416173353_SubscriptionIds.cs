using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sameposty.DataAccess.Migrations;

/// <inheritdoc />
public partial class SubscriptionIds : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "StripeCustomerId",
            table: "Users");

        migrationBuilder.AddColumn<string>(
            name: "StripeCustomerId",
            table: "Subscriptions",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "StripeCustomerId",
            table: "Subscriptions");

        migrationBuilder.AddColumn<string>(
            name: "StripeCustomerId",
            table: "Users",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");
    }
}
