using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AddUserToBudgetCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "BudgetCategories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetCategories_ApplicationUserId",
                table: "BudgetCategories",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetCategories_AspNetUsers_ApplicationUserId",
                table: "BudgetCategories",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetCategories_AspNetUsers_ApplicationUserId",
                table: "BudgetCategories");

            migrationBuilder.DropIndex(
                name: "IX_BudgetCategories_ApplicationUserId",
                table: "BudgetCategories");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "BudgetCategories");
        }
    }
}
