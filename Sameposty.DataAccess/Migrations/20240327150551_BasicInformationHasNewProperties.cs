using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sameposty.DataAccess.Migrations;

/// <inheritdoc />
public partial class BasicInformationHasNewProperties : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "ProductsAndServices",
            table: "BasicInformations",
            type: "nvarchar(1000)",
            maxLength: 1000,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(500)",
            oldMaxLength: 500);

        migrationBuilder.AlterColumn<string>(
            name: "Goals",
            table: "BasicInformations",
            type: "nvarchar(1000)",
            maxLength: 1000,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(500)",
            oldMaxLength: 500);

        migrationBuilder.AlterColumn<string>(
            name: "Assets",
            table: "BasicInformations",
            type: "nvarchar(1000)",
            maxLength: 1000,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(500)",
            oldMaxLength: 500);

        migrationBuilder.AddColumn<string>(
            name: "Audience",
            table: "BasicInformations",
            type: "nvarchar(1000)",
            maxLength: 1000,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "BrandName",
            table: "BasicInformations",
            type: "nvarchar(1000)",
            maxLength: 1000,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "Mission",
            table: "BasicInformations",
            type: "nvarchar(1000)",
            maxLength: 1000,
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Audience",
            table: "BasicInformations");

        migrationBuilder.DropColumn(
            name: "BrandName",
            table: "BasicInformations");

        migrationBuilder.DropColumn(
            name: "Mission",
            table: "BasicInformations");

        migrationBuilder.AlterColumn<string>(
            name: "ProductsAndServices",
            table: "BasicInformations",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(1000)",
            oldMaxLength: 1000);

        migrationBuilder.AlterColumn<string>(
            name: "Goals",
            table: "BasicInformations",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(1000)",
            oldMaxLength: 1000);

        migrationBuilder.AlterColumn<string>(
            name: "Assets",
            table: "BasicInformations",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(1000)",
            oldMaxLength: 1000);
    }
}
