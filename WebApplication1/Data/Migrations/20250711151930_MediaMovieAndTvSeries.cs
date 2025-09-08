using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class MediaMovieAndTvSeries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Directors_directorId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Genres_genreId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Movies_MovieId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMovieLike_Movies_movieId",
                table: "UserMovieLike");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movies",
                table: "Movies");

            migrationBuilder.RenameTable(
                name: "Movies",
                newName: "Medias");

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

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "Reviews",
                newName: "MediaId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_MovieId",
                table: "Reviews",
                newName: "IX_Reviews_MediaId");

            migrationBuilder.RenameIndex(
                name: "IX_Movies_genreId",
                table: "Medias",
                newName: "IX_Medias_genreId");

            migrationBuilder.RenameIndex(
                name: "IX_Movies_directorId",
                table: "Medias",
                newName: "IX_Medias_directorId");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Medias",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Episodes",
                table: "Medias",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCinemaRelease",
                table: "Medias",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Medias",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MediaType",
                table: "Medias",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Network",
                table: "Medias",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Medias",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Seasons",
                table: "Medias",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Medias",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Medias",
                table: "Medias",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Directors_directorId",
                table: "Medias",
                column: "directorId",
                principalTable: "Directors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Genres_genreId",
                table: "Medias",
                column: "genreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Medias_MediaId",
                table: "Reviews",
                column: "MediaId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMovieLike_Medias_mediaId",
                table: "UserMovieLike",
                column: "mediaId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Directors_directorId",
                table: "Medias");

            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Genres_genreId",
                table: "Medias");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Medias_MediaId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMovieLike_Medias_mediaId",
                table: "UserMovieLike");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Medias",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "Episodes",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "IsCinemaRelease",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "Network",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "Seasons",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Medias");

            migrationBuilder.RenameTable(
                name: "Medias",
                newName: "Movies");

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

            migrationBuilder.RenameColumn(
                name: "MediaId",
                table: "Reviews",
                newName: "MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_MediaId",
                table: "Reviews",
                newName: "IX_Reviews_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Medias_genreId",
                table: "Movies",
                newName: "IX_Movies_genreId");

            migrationBuilder.RenameIndex(
                name: "IX_Medias_directorId",
                table: "Movies",
                newName: "IX_Movies_directorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movies",
                table: "Movies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Directors_directorId",
                table: "Movies",
                column: "directorId",
                principalTable: "Directors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Genres_genreId",
                table: "Movies",
                column: "genreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Movies_MovieId",
                table: "Reviews",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMovieLike_Movies_movieId",
                table: "UserMovieLike",
                column: "movieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
