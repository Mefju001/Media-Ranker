using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class FixedLikedMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "MediaId",
                table: "LikedMedias",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LikedMedias_MediaId",
                table: "LikedMedias",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_LikedMedias_UserId_MediaId",
                table: "LikedMedias",
                columns: new[] { "UserId", "MediaId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LikedMedias_Medias_MediaId",
                table: "LikedMedias",
                column: "MediaId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikedMedias_Medias_MediaId",
                table: "LikedMedias");

            migrationBuilder.DropIndex(
                name: "IX_LikedMedias_MediaId",
                table: "LikedMedias");

            migrationBuilder.DropIndex(
                name: "IX_LikedMedias_UserId_MediaId",
                table: "LikedMedias");

            migrationBuilder.DropColumn(
                name: "MediaId",
                table: "LikedMedias");

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
        }
    }
}
