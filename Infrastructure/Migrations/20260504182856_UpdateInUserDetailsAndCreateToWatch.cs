using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInUserDetailsAndCreateToWatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LikedMedias",
                table: "LikedMedias");

            migrationBuilder.DropIndex(
                name: "IX_LikedMedias_UserId_MediaId",
                table: "LikedMedias");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "UsersDetails",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "UsersDetails",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LikedMedias",
                table: "LikedMedias",
                columns: new[] { "UserId", "MediaId" });

            migrationBuilder.CreateTable(
                name: "ToWatch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    LikedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
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
                name: "IX_ToWatch_UserId",
                table: "ToWatch",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToWatch");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LikedMedias",
                table: "LikedMedias");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "UsersDetails");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "UsersDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LikedMedias",
                table: "LikedMedias",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_LikedMedias_UserId_MediaId",
                table: "LikedMedias",
                columns: new[] { "UserId", "MediaId" },
                unique: true);
        }
    }
}
