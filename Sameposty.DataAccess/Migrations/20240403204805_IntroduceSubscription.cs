using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sameposty.DataAccess.Migrations;

/// <inheritdoc />
public partial class IntroduceSubscription : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Subscriptions",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CustomerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                OrderId = table.Column<int>(type: "int", nullable: false),
                AmountPaid = table.Column<double>(type: "float", nullable: false),
                OrderHasInvoice = table.Column<bool>(type: "bit", nullable: false),
                SubscriptionCurrentPeriodStart = table.Column<string>(type: "nvarchar(max)", nullable: false),
                SubscriptionCurrentPeriodEnd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                UserId = table.Column<int>(type: "int", nullable: true),
                CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Subscriptions", x => x.Id);
                table.ForeignKey(
                    name: "FK_Subscriptions_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Subscriptions_UserId",
            table: "Subscriptions",
            column: "UserId",
            unique: true,
            filter: "[UserId] IS NOT NULL");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Subscriptions");
    }
}
