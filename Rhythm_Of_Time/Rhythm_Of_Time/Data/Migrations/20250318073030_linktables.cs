using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rhythm_Of_Time.Data.Migrations
{
    /// <inheritdoc />
    public partial class linktables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "artistSongs",
                columns: table => new
                {
                    ArtistSong_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SongId = table.Column<int>(type: "int", nullable: false),
                    ArtistId = table.Column<int>(type: "int", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_artistSongs", x => x.ArtistSong_Id);
                    table.ForeignKey(
                        name: "FK_artistSongs_artist_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "artist",
                        principalColumn: "ArtistId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_artistSongs_song_SongId",
                        column: x => x.SongId,
                        principalTable: "song",
                        principalColumn: "SongId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "awardSongs",
                columns: table => new
                {
                    AwardSong_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SongId = table.Column<int>(type: "int", nullable: false),
                    AwardId = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_awardSongs", x => x.AwardSong_Id);
                    table.ForeignKey(
                        name: "FK_awardSongs_award_AwardId",
                        column: x => x.AwardId,
                        principalTable: "award",
                        principalColumn: "AwardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_awardSongs_song_SongId",
                        column: x => x.SongId,
                        principalTable: "song",
                        principalColumn: "SongId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_artistSongs_ArtistId",
                table: "artistSongs",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_artistSongs_SongId",
                table: "artistSongs",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_awardSongs_AwardId",
                table: "awardSongs",
                column: "AwardId");

            migrationBuilder.CreateIndex(
                name: "IX_awardSongs_SongId",
                table: "awardSongs",
                column: "SongId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "artistSongs");

            migrationBuilder.DropTable(
                name: "awardSongs");
        }
    }
}
