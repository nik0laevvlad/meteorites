using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestTask.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Meteorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameType = table.Column<int>(type: "int", nullable: true),
                    RecClass = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Mass = table.Column<double>(type: "float", nullable: true),
                    Fall = table.Column<int>(type: "int", nullable: true),
                    Year = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecLat = table.Column<double>(type: "float", nullable: true),
                    RecLong = table.Column<double>(type: "float", nullable: true),
                    Geo_Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Geo_Lon = table.Column<double>(type: "float", nullable: true),
                    Geo_Lat = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meteorites", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meteorites_Name",
                table: "Meteorites",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Meteorites_RecClass",
                table: "Meteorites",
                column: "RecClass");

            migrationBuilder.CreateIndex(
                name: "IX_Meteorites_Year",
                table: "Meteorites",
                column: "Year");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meteorites");
        }
    }
}
