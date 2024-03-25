using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShuffleLit.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordChangedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordChangedDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordChangedDate",
                table: "AspNetUsers");
        }
    }
}
