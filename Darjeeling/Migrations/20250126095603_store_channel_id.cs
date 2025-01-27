using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Darjeeling.Migrations
{
    /// <inheritdoc />
    public partial class store_channel_id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NameHistories_FCMembers_FCMemberId",
                table: "NameHistories");

            migrationBuilder.DropIndex(
                name: "IX_NameHistories_FCMemberId",
                table: "NameHistories");

            migrationBuilder.AddColumn<int>(
                name: "FcGuildMemberId",
                table: "NameHistories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AdminChannelId",
                table: "FCGuildServers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_NameHistories_FcGuildMemberId",
                table: "NameHistories",
                column: "FcGuildMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_NameHistories_FCMembers_FcGuildMemberId",
                table: "NameHistories",
                column: "FcGuildMemberId",
                principalTable: "FCMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NameHistories_FCMembers_FcGuildMemberId",
                table: "NameHistories");

            migrationBuilder.DropIndex(
                name: "IX_NameHistories_FcGuildMemberId",
                table: "NameHistories");

            migrationBuilder.DropColumn(
                name: "FcGuildMemberId",
                table: "NameHistories");

            migrationBuilder.DropColumn(
                name: "AdminChannelId",
                table: "FCGuildServers");

            migrationBuilder.CreateIndex(
                name: "IX_NameHistories_FCMemberId",
                table: "NameHistories",
                column: "FCMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_NameHistories_FCMembers_FCMemberId",
                table: "NameHistories",
                column: "FCMemberId",
                principalTable: "FCMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
