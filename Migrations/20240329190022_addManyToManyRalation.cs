using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShuffleLit.Migrations
{
    /// <inheritdoc />
    public partial class addManyToManyRalation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LiteratureCollections",
                columns: table => new
                {
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LiteratureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiteratureCollections", x => new { x.AppUserId, x.LiteratureId });
                    table.ForeignKey(
                        name: "FK_LiteratureCollections_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LiteratureCollections_Literatures_LiteratureId",
                        column: x => x.LiteratureId,
                        principalTable: "Literatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LiteratureCollections_LiteratureId",
                table: "LiteratureCollections",
                column: "LiteratureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LiteratureCollections");
        }
    }
}
