using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sameposty.DataAccess.Migrations;

/// <inheritdoc />
public partial class InvoiceHasLong : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<long>(
            name: "FakturowniaClientId",
            table: "Invoices",
            type: "bigint",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "FakturowniaClientId",
            table: "Invoices",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(long),
            oldType: "bigint");
    }
}
