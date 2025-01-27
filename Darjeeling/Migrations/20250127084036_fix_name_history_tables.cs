using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Darjeeling.Migrations
{
    /// <inheritdoc />
    public partial class fix_name_history_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NameHistories");

            migrationBuilder.DropColumn(
                name: "DiscordUsername",
                table: "FCMembers");

            migrationBuilder.CreateTable(
                name: "DiscordNameHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscordUsername = table.Column<string>(type: "text", nullable: false),
                    DiscordNickName = table.Column<string>(type: "text", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FcGuildMemberId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordNameHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordNameHistories_FCMembers_FcGuildMemberId",
                        column: x => x.FcGuildMemberId,
                        principalTable: "FCMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LodestoneNameHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FCMemberId = table.Column<int>(type: "integer", nullable: false),
                    FcGuildMemberId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LodestoneNameHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LodestoneNameHistories_FCMembers_FcGuildMemberId",
                        column: x => x.FcGuildMemberId,
                        principalTable: "FCMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscordNameHistories_FcGuildMemberId",
                table: "DiscordNameHistories",
                column: "FcGuildMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_LodestoneNameHistories_FcGuildMemberId",
                table: "LodestoneNameHistories",
                column: "FcGuildMemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscordNameHistories");

            migrationBuilder.DropTable(
                name: "LodestoneNameHistories");

            migrationBuilder.AddColumn<string>(
                name: "DiscordUsername",
                table: "FCMembers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NameHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FcGuildMemberId = table.Column<int>(type: "integer", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DiscordUserUid = table.Column<string>(type: "text", nullable: true),
                    FCMemberId = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NameHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NameHistories_FCMembers_FcGuildMemberId",
                        column: x => x.FcGuildMemberId,
                        principalTable: "FCMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NameHistories_FcGuildMemberId",
                table: "NameHistories",
                column: "FcGuildMemberId");
        }
    }
}
