using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinecraftE_Commerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class bug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Announcements",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_UserId1",
                table: "Announcements",
                column: "UserId1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_AspNetUsers_UserId1",
                table: "Announcements",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_AspNetUsers_UserId1",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_UserId1",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Announcements");
        }
    }
}
