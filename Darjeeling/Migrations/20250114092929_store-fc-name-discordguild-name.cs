using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Darjeeling.Migrations
{
    /// <inheritdoc />
    public partial class storefcnamediscordguildname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiscordGuildName",
                table: "FCGuildServers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FreeCompanyName",
                table: "FCGuildServers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscordGuildName",
                table: "FCGuildServers");

            migrationBuilder.DropColumn(
                name: "FreeCompanyName",
                table: "FCGuildServers");
        }
    }
}
