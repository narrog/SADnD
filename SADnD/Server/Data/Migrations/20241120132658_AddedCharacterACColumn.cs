using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SADnD.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedCharacterACColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArmorClass",
                table: "Characters",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArmorClass",
                table: "Characters");
        }
    }
}
