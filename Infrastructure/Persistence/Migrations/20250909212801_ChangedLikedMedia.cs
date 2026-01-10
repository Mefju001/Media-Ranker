using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class ChangedLikedMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikedMedias_Medias_mediaId",
                table: "LikedMedias");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedMedias_Users_userId",
                table: "LikedMedias");

            migrationBuilder.DropIndex(
                name: "IX_LikedMedias_mediaId",
                table: "LikedMedias");

            migrationBuilder.DropIndex(
                name: "IX_LikedMedias_userId_mediaId",
                table: "LikedMedias");

            migrationBuilder.DropColumn(
                name: "mediaId",
                table: "LikedMedias");

            migrationBuilder.DropColumn(
                name: "mediaType",
                table: "LikedMedias");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "LikedMedias",
                newName: "UserId");

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "LikedMedias",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "LikedMedias",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TvSeriesId",
                table: "LikedMedias",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "LikedMedias",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_LikedMedias_GameId",
                table: "LikedMedias",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_LikedMedias_MovieId",
                table: "LikedMedias",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_LikedMedias_TvSeriesId",
                table: "LikedMedias",
                column: "TvSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_LikedMedias_UserId_MovieId_TvSeriesId_GameId",
                table: "LikedMedias",
                columns: new[] { "UserId", "MovieId", "TvSeriesId", "GameId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LikedMedias_Medias_GameId",
                table: "LikedMedias",
                column: "GameId",
                principalTable: "Medias",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LikedMedias_Medias_MovieId",
                table: "LikedMedias",
                column: "MovieId",
                principalTable: "Medias",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LikedMedias_Medias_TvSeriesId",
                table: "LikedMedias",
                column: "TvSeriesId",
                principalTable: "Medias",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LikedMedias_Users_UserId",
                table: "LikedMedias",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikedMedias_Medias_GameId",
                table: "LikedMedias");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedMedias_Medias_MovieId",
                table: "LikedMedias");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedMedias_Medias_TvSeriesId",
                table: "LikedMedias");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedMedias_Users_UserId",
                table: "LikedMedias");

            migrationBuilder.DropIndex(
                name: "IX_LikedMedias_GameId",
                table: "LikedMedias");

            migrationBuilder.DropIndex(
                name: "IX_LikedMedias_MovieId",
                table: "LikedMedias");

            migrationBuilder.DropIndex(
                name: "IX_LikedMedias_TvSeriesId",
                table: "LikedMedias");

            migrationBuilder.DropIndex(
                name: "IX_LikedMedias_UserId_MovieId_TvSeriesId_GameId",
                table: "LikedMedias");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "LikedMedias");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "LikedMedias");

            migrationBuilder.DropColumn(
                name: "TvSeriesId",
                table: "LikedMedias");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "LikedMedias");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "LikedMedias",
                newName: "userId");

            migrationBuilder.AddColumn<int>(
                name: "mediaId",
                table: "LikedMedias",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "mediaType",
                table: "LikedMedias",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LikedMedias_mediaId",
                table: "LikedMedias",
                column: "mediaId");

            migrationBuilder.CreateIndex(
                name: "IX_LikedMedias_userId_mediaId",
                table: "LikedMedias",
                columns: new[] { "userId", "mediaId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LikedMedias_Medias_mediaId",
                table: "LikedMedias",
                column: "mediaId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LikedMedias_Users_userId",
                table: "LikedMedias",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
