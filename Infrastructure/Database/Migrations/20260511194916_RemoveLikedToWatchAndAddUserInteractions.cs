using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLikedToWatchAndAddUserInteractions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LikedMedias");

            migrationBuilder.DropTable(
                name: "ToWatch");

            migrationBuilder.CreateTable(
                name: "UserInteractions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    RatingVote = table.Column<int>(type: "integer", nullable: true),
                    TypeInteractions = table.Column<int>(type: "integer", nullable: true),
                    InteractionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserDetailsId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInteractions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInteractions_UsersDetails_UserDetailsId",
                        column: x => x.UserDetailsId,
                        principalTable: "UsersDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInteractions_UserDetailsId",
                table: "UserInteractions",
                column: "UserDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInteractions_UserId_MediaId",
                table: "UserInteractions",
                columns: new[] { "UserId", "MediaId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInteractions");

            migrationBuilder.CreateTable(
                name: "LikedMedias",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LikedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikedMedias", x => new { x.UserId, x.MediaId });
                    table.ForeignKey(
                        name: "FK_LikedMedias_Medias_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LikedMedias_UsersDetails_UserId",
                        column: x => x.UserId,
                        principalTable: "UsersDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ToWatch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LikedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToWatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToWatch_UsersDetails_UserId",
                        column: x => x.UserId,
                        principalTable: "UsersDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LikedMedias_MediaId",
                table: "LikedMedias",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_ToWatch_UserId",
                table: "ToWatch",
                column: "UserId");
        }
    }
}
