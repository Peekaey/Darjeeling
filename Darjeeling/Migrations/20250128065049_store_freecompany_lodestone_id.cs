using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Darjeeling.Migrations
{
    /// <inheritdoc />
    public partial class store_freecompany_lodestone_id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FreeCompanyId",
                table: "FCGuildServers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FreeCompanyId",
                table: "FCGuildServers");
        }
    }
}
