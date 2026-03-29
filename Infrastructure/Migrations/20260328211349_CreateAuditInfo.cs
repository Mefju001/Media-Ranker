using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateAuditInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikedMedias_AspNetUsers_userId",
                table: "LikedMedias");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedMedias_Medias_mediaId",
                table: "LikedMedias");

            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Directors_DirectorId",
                table: "Medias");

            migrationBuilder.DropTable(
                name: "MediaStats");

            migrationBuilder.DropIndex(
                name: "IX_Medias_DirectorId",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "refreshToken",
                table: "Tokens",
                newName: "RefreshToken");

            migrationBuilder.RenameColumn(
                name: "Jti",
                table: "Tokens",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ReleaseDate",
                table: "Medias",
                newName: "ReleaseYear");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Medias",
                newName: "DurationMinutes");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "LikedMedias",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "mediaId",
                table: "LikedMedias",
                newName: "MediaId");

            migrationBuilder.RenameColumn(
                name: "likedDate",
                table: "LikedMedias",
                newName: "LikedDate");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "LikedMedias",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_LikedMedias_userId_mediaId",
                table: "LikedMedias",
                newName: "IX_LikedMedias_UserId_MediaId");

            migrationBuilder.RenameIndex(
                name: "IX_LikedMedias_mediaId",
                table: "LikedMedias",
                newName: "IX_LikedMedias_MediaId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Genres",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "surname",
                table: "Directors",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Directors",
                newName: "Name");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Reviews",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Medias",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Platform",
                table: "Medias",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Network",
                table: "Medias",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Developer",
                table: "Medias",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "Medias",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Medias",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Stats_LastCalculated",
                table: "Medias",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Medias",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "reviewCount",
                table: "Medias",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MediaId1",
                table: "LikedMedias",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Genres",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AspNetUsers",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LikedMedias_MediaId1",
                table: "LikedMedias",
                column: "MediaId1");

            migrationBuilder.AddForeignKey(
                name: "FK_LikedMedias_AspNetUsers_UserId",
                table: "LikedMedias",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LikedMedias_Medias_MediaId",
                table: "LikedMedias",
                column: "MediaId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LikedMedias_Medias_MediaId1",
                table: "LikedMedias",
                column: "MediaId1",
                principalTable: "Medias",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikedMedias_AspNetUsers_UserId",
                table: "LikedMedias");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedMedias_Medias_MediaId",
                table: "LikedMedias");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedMedias_Medias_MediaId1",
                table: "LikedMedias");

            migrationBuilder.DropIndex(
                name: "IX_LikedMedias_MediaId1",
                table: "LikedMedias");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "Stats_LastCalculated",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "reviewCount",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "MediaId1",
                table: "LikedMedias");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                table: "Tokens",
                newName: "refreshToken");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Tokens",
                newName: "Jti");

            migrationBuilder.RenameColumn(
                name: "ReleaseYear",
                table: "Medias",
                newName: "ReleaseDate");

            migrationBuilder.RenameColumn(
                name: "DurationMinutes",
                table: "Medias",
                newName: "Duration");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "LikedMedias",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "MediaId",
                table: "LikedMedias",
                newName: "mediaId");

            migrationBuilder.RenameColumn(
                name: "LikedDate",
                table: "LikedMedias",
                newName: "likedDate");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "LikedMedias",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_LikedMedias_UserId_MediaId",
                table: "LikedMedias",
                newName: "IX_LikedMedias_userId_mediaId");

            migrationBuilder.RenameIndex(
                name: "IX_LikedMedias_MediaId",
                table: "LikedMedias",
                newName: "IX_LikedMedias_mediaId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Genres",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "Directors",
                newName: "surname");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Directors",
                newName: "name");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Reviews",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Medias",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Platform",
                table: "Medias",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Network",
                table: "Medias",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Developer",
                table: "Medias",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Genres",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateTable(
                name: "MediaStats",
                columns: table => new
                {
                    MediaId = table.Column<int>(type: "integer", nullable: false),
                    AverageRating = table.Column<double>(type: "double precision", nullable: true),
                    LastCalculated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ReviewCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaStats", x => x.MediaId);
                    table.ForeignKey(
                        name: "FK_MediaStats_Medias_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medias_DirectorId",
                table: "Medias",
                column: "DirectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_LikedMedias_AspNetUsers_userId",
                table: "LikedMedias",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LikedMedias_Medias_mediaId",
                table: "LikedMedias",
                column: "mediaId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Directors_DirectorId",
                table: "Medias",
                column: "DirectorId",
                principalTable: "Directors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
