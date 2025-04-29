using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetIsland.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class update_contact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ORS_lat",
                table: "Contact",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ORS_lon",
                table: "Contact",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ORS_lat",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "ORS_lon",
                table: "Contact");
        }
    }
}
