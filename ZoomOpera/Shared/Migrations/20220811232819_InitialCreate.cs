using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZoomOpera.Shared.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GivenName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuildingCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Levels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LevelNumber = table.Column<int>(type: "int", nullable: false),
                    Planimetry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Levels_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocationCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonitorPlatforms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MonitorCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitorPlatforms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonitorPlatforms_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItalianDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operas_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperaImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperaImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperaImages_Operas_OperaId",
                        column: x => x.OperaId,
                        principalTable: "Operas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageMaps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageMapShape = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperaImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DetailedDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageMaps_OperaImages_OperaImageId",
                        column: x => x.OperaImageId,
                        principalTable: "OperaImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageMapCoordinates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    X = table.Column<double>(type: "float", nullable: false),
                    Y = table.Column<double>(type: "float", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    ImageMapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageMapCoordinates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageMapCoordinates_ImageMaps_ImageMapId",
                        column: x => x.ImageMapId,
                        principalTable: "ImageMaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageMapCoordinates_ImageMapId",
                table: "ImageMapCoordinates",
                column: "ImageMapId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageMaps_OperaImageId",
                table: "ImageMaps",
                column: "OperaImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Levels_BuildingId",
                table: "Levels",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LevelId",
                table: "Locations",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_MonitorPlatforms_LevelId",
                table: "MonitorPlatforms",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_OperaImages_OperaId",
                table: "OperaImages",
                column: "OperaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Operas_LocationId",
                table: "Operas",
                column: "LocationId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "ImageMapCoordinates");

            migrationBuilder.DropTable(
                name: "MonitorPlatforms");

            migrationBuilder.DropTable(
                name: "ImageMaps");

            migrationBuilder.DropTable(
                name: "OperaImages");

            migrationBuilder.DropTable(
                name: "Operas");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Levels");

            migrationBuilder.DropTable(
                name: "Buildings");
        }
    }
}
