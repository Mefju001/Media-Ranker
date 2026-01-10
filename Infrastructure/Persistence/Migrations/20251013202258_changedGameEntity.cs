using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class changedGameEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "Medias");
            migrationBuilder.Sql(
                @"ALTER TABLE ""Medias""
              ALTER COLUMN ""Platform"" TYPE integer USING CASE 
                WHEN ""Platform"" = 'PC' THEN 1
                WHEN ""Platform"" = 'Playstation' THEN 2
                WHEN ""Platform"" = 'Xbox' THEN 3
                ELSE 0 -- Wartość domyślna dla nieznanych/nulli, jeśli są
              END;"
            );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Platform",
                table: "Medias",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "Medias",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
