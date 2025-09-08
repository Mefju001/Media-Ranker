using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class updateLikedMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMovieLike");

            migrationBuilder.CreateTable(
                name: "LikedMedias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userId = table.Column<int>(type: "integer", nullable: false),
                    mediaId = table.Column<int>(type: "integer", nullable: false),
                    mediaType = table.Column<int>(type: "integer", nullable: false),
                    LikedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikedMedias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LikedMedias_Medias_mediaId",
                        column: x => x.mediaId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LikedMedias_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LikedMedias_mediaId",
                table: "LikedMedias",
                column: "mediaId");

            migrationBuilder.CreateIndex(
                name: "IX_LikedMedias_userId_mediaId",
                table: "LikedMedias",
                columns: new[] { "userId", "mediaId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LikedMedias");

            migrationBuilder.CreateTable(
                name: "UserMovieLike",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    movieId = table.Column<int>(type: "integer", nullable: false),
                    userId = table.Column<int>(type: "integer", nullable: false),
                    LikedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMovieLike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMovieLike_Medias_movieId",
                        column: x => x.movieId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMovieLike_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserMovieLike_movieId",
                table: "UserMovieLike",
                column: "movieId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMovieLike_userId_movieId",
                table: "UserMovieLike",
                columns: new[] { "userId", "movieId" },
                unique: true);
        }
    }
}
