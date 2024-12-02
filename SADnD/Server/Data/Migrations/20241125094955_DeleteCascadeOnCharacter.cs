using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SADnD.Server.Migrations
{
    /// <inheritdoc />
    public partial class DeleteCascadeOnCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteHint_Characters_CharacterId",
                table: "NoteHint");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteLocation_Characters_CharacterId",
                table: "NoteLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_NotePerson_Characters_CharacterId",
                table: "NotePerson");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteQuest_Characters_CharacterId",
                table: "NoteQuest");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteStory_Characters_CharacterId",
                table: "NoteStory");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "NoteStory",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "NoteQuest",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "NotePerson",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "NoteLocation",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "NoteHint",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteHint_Characters_CharacterId",
                table: "NoteHint",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteLocation_Characters_CharacterId",
                table: "NoteLocation",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotePerson_Characters_CharacterId",
                table: "NotePerson",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteQuest_Characters_CharacterId",
                table: "NoteQuest",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteStory_Characters_CharacterId",
                table: "NoteStory",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteHint_Characters_CharacterId",
                table: "NoteHint");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteLocation_Characters_CharacterId",
                table: "NoteLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_NotePerson_Characters_CharacterId",
                table: "NotePerson");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteQuest_Characters_CharacterId",
                table: "NoteQuest");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteStory_Characters_CharacterId",
                table: "NoteStory");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "NoteStory",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "NoteQuest",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "NotePerson",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "NoteLocation",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "NoteHint",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteHint_Characters_CharacterId",
                table: "NoteHint",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteLocation_Characters_CharacterId",
                table: "NoteLocation",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotePerson_Characters_CharacterId",
                table: "NotePerson",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteQuest_Characters_CharacterId",
                table: "NoteQuest",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteStory_Characters_CharacterId",
                table: "NoteStory",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id");
        }
    }
}
