using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MariBot.Data.Migrations;

public partial class CreatedGuildTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "guilds",
            columns: table => new
            {
                id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                name = table.Column<string>(type: "text", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_guilds", x => x.id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "guilds");
    }
}
