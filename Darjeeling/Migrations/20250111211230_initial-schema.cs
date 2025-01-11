using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Darjeeling.Migrations
{
    /// <inheritdoc />
    public partial class initialschema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FCGuildServers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AdminRoleId = table.Column<string>(type: "text", nullable: false),
                    DiscordGuildUid = table.Column<string>(type: "text", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FCGuildServers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FCGuildRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    RoleName = table.Column<string>(type: "text", nullable: false),
                    DiscordGuildUid = table.Column<string>(type: "text", nullable: false),
                    FreeCompanyId = table.Column<string>(type: "text", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RoleType = table.Column<int>(type: "integer", nullable: false),
                    FCGuildServerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FCGuildRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FCGuildRoles_FCGuildServers_FCGuildServerId",
                        column: x => x.FCGuildServerId,
                        principalTable: "FCGuildServers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FCMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscordUserUId = table.Column<string>(type: "text", nullable: false),
                    DiscordUsername = table.Column<string>(type: "text", nullable: false),
                    LodestoneId = table.Column<string>(type: "text", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FCGuildServerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FCMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FCMembers_FCGuildServers_FCGuildServerId",
                        column: x => x.FCGuildServerId,
                        principalTable: "FCGuildServers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NameHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscordUserUid = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FCMemberId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NameHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NameHistories_FCMembers_FCMemberId",
                        column: x => x.FCMemberId,
                        principalTable: "FCMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FCGuildRoles_FCGuildServerId",
                table: "FCGuildRoles",
                column: "FCGuildServerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FCMembers_FCGuildServerId",
                table: "FCMembers",
                column: "FCGuildServerId");

            migrationBuilder.CreateIndex(
                name: "IX_NameHistories_FCMemberId",
                table: "NameHistories",
                column: "FCMemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FCGuildRoles");

            migrationBuilder.DropTable(
                name: "NameHistories");

            migrationBuilder.DropTable(
                name: "FCMembers");

            migrationBuilder.DropTable(
                name: "FCGuildServers");
        }
    }
}
