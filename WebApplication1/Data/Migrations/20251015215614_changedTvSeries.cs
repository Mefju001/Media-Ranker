using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class changedTvSeries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            @"ALTER TABLE ""Medias""
              ALTER COLUMN ""Status"" TYPE integer USING CASE 
                WHEN ""Status"" = 'Completed' THEN 1
                WHEN ""Status"" = 'Playstation' THEN 2
                WHEN ""Status"" = 'Xbox' THEN 3
                ELSE 0 -- Wartość domyślna dla nieznanych/nulli, jeśli są
              END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Medias",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
