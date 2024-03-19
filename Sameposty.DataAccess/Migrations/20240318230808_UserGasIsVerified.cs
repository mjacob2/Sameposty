﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sameposty.DataAccess.Migrations;

/// <inheritdoc />
public partial class UserGasIsVerified : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsVerified",
            table: "Users",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsVerified",
            table: "Users");
    }
}
