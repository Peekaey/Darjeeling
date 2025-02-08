using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Darjeeling.Migrations
{
    /// <inheritdoc />
    public partial class add_DiscordNameHistory_GuildNickname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiscordGuildNickname",
                table: "DiscordNameHistories",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscordGuildNickname",
                table: "DiscordNameHistories");
        }
    }
}
