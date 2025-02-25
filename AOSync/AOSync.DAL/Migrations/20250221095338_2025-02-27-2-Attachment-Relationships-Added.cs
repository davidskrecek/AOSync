using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AOSync.DAL.Migrations
{
    /// <inheritdoc />
    public partial class _202502272AttachmentRelationshipsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Teams_TeamId",
                table: "Attachments");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Teams_TeamId",
                table: "Attachments",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Teams_TeamId",
                table: "Attachments");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Teams_TeamId",
                table: "Attachments",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }
    }
}
