using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWishItemStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WishItems_AspNetUsers_ApplicationUserId",
                table: "WishItems");

            migrationBuilder.DropColumn(
                name: "DesiredAmount",
                table: "WishItems");

            migrationBuilder.RenameColumn(
                name: "IsAffordable",
                table: "WishItems",
                newName: "IsCompleted");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "WishItems",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "WishItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

            migrationBuilder.AddColumn<decimal>(
                name: "TargetAmount",
                table: "WishItems",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WishItems_AspNetUsers_ApplicationUserId",
                table: "WishItems",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WishItems_AspNetUsers_ApplicationUserId",
                table: "WishItems");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "WishItems");

            migrationBuilder.DropColumn(
                name: "IsAchievable",
                table: "WishItems");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "WishItems");

            migrationBuilder.DropColumn(
                name: "TargetAmount",
                table: "WishItems");

            migrationBuilder.RenameColumn(
                name: "IsCompleted",
                table: "WishItems",
                newName: "IsAffordable");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "WishItems",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DesiredAmount",
                table: "WishItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_WishItems_AspNetUsers_ApplicationUserId",
                table: "WishItems",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
