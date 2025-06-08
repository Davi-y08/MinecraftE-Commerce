using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinecraftE_Commerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class chatIdAnnouncement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Sales_SaleId",
                table: "Chats");

            migrationBuilder.AlterColumn<int>(
                name: "SaleId",
                table: "Chats",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AnnouncementId",
                table: "Chats",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Sales_SaleId",
                table: "Chats",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Sales_SaleId",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "AnnouncementId",
                table: "Chats");

            migrationBuilder.AlterColumn<int>(
                name: "SaleId",
                table: "Chats",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Sales_SaleId",
                table: "Chats",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
