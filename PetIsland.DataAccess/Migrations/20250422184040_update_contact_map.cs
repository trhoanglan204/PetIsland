using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetIsland.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class update_contact_map : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Map",
                table: "Contact",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Map",
                table: "Contact");
        }
    }
}
