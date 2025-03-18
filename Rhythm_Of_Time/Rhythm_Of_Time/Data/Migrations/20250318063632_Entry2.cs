using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rhythm_Of_Time.Data.Migrations
{
    /// <inheritdoc />
    public partial class Entry2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "entry",
                columns: table => new
                {
                    entry_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    timeline_Id = table.Column<int>(type: "int", nullable: false),
                    SongId = table.Column<int>(type: "int", nullable: false),
                    decription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entry", x => x.entry_Id);
                    table.ForeignKey(
                        name: "FK_entry_song_SongId",
                        column: x => x.SongId,
                        principalTable: "song",
                        principalColumn: "SongId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_entry_timelines_timeline_Id",
                        column: x => x.timeline_Id,
                        principalTable: "timelines",
                        principalColumn: "timeline_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_entry_SongId",
                table: "entry",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_entry_timeline_Id",
                table: "entry",
                column: "timeline_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "entry");
        }
    }
}
