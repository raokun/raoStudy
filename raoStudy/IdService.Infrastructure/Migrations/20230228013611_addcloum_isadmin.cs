using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addcloum_isadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "sys_rule",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "sys_rule");
        }
    }
}
