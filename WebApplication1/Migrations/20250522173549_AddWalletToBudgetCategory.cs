using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AddWalletToBudgetCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WalletId",
                table: "BudgetCategories",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetCategories_WalletId",
                table: "BudgetCategories",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetCategories_Wallets_WalletId",
                table: "BudgetCategories",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetCategories_Wallets_WalletId",
                table: "BudgetCategories");

            migrationBuilder.DropIndex(
                name: "IX_BudgetCategories_WalletId",
                table: "BudgetCategories");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "BudgetCategories");
        }
    }
}
