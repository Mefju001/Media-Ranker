using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class RefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMovieLike_Medias_mediaId",
                table: "UserMovieLike");

            migrationBuilder.RenameColumn(
                name: "mediaId",
                table: "UserMovieLike",
                newName: "movieId");

            migrationBuilder.RenameIndex(
                name: "IX_UserMovieLike_userId_mediaId",
                table: "UserMovieLike",
                newName: "IX_UserMovieLike_userId_movieId");

            migrationBuilder.RenameIndex(
                name: "IX_UserMovieLike_mediaId",
                table: "UserMovieLike",
                newName: "IX_UserMovieLike_movieId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMovieLike_Medias_movieId",
                table: "UserMovieLike",
                column: "movieId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMovieLike_Medias_movieId",
                table: "UserMovieLike");

            migrationBuilder.RenameColumn(
                name: "movieId",
                table: "UserMovieLike",
                newName: "mediaId");

            migrationBuilder.RenameIndex(
                name: "IX_UserMovieLike_userId_movieId",
                table: "UserMovieLike",
                newName: "IX_UserMovieLike_userId_mediaId");

            migrationBuilder.RenameIndex(
                name: "IX_UserMovieLike_movieId",
                table: "UserMovieLike",
                newName: "IX_UserMovieLike_mediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMovieLike_Medias_mediaId",
                table: "UserMovieLike",
                column: "mediaId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
