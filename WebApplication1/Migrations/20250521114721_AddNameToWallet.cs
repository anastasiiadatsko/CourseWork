using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AddNameToWallet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAchievable",
                table: "WishItems");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "WishItems");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Wallets",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Wallets");

            migrationBuilder.AddColumn<bool>(
                name: "IsAchievable",
                table: "WishItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Month",
                table: "WishItems",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
