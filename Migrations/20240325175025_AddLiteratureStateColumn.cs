using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShuffleLit.Migrations
{
    /// <inheritdoc />
    public partial class AddLiteratureStateColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LiteratureState",
                table: "Literatures",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LiteratureState",
                table: "Literatures");
        }
    }
}
