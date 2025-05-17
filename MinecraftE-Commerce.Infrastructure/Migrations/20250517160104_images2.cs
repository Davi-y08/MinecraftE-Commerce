using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinecraftE_Commerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class images2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImagesAnnouncements_Announcements_AnnouncementId1",
                table: "ImagesAnnouncements");

            migrationBuilder.DropIndex(
                name: "IX_ImagesAnnouncements_AnnouncementId1",
                table: "ImagesAnnouncements");

            migrationBuilder.DropColumn(
                name: "AnnouncementId1",
                table: "ImagesAnnouncements");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnnouncementId1",
                table: "ImagesAnnouncements",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImagesAnnouncements_AnnouncementId1",
                table: "ImagesAnnouncements",
                column: "AnnouncementId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ImagesAnnouncements_Announcements_AnnouncementId1",
                table: "ImagesAnnouncements",
                column: "AnnouncementId1",
                principalTable: "Announcements",
                principalColumn: "Id");
        }
    }
}
