using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Darjeeling.Migrations
{
    /// <inheritdoc />
    public partial class remove_FreeCompanyId_From_FCGuildRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FreeCompanyId",
                table: "FCGuildRoles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FreeCompanyId",
                table: "FCGuildRoles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
