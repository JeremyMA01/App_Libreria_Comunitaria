using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Libreria_Comunitaria.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveToDonations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Donations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Donations");
        }
    }
}
