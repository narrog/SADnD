using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SADnD.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedNoteTypeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteHint_AspNetUsers_UserId",
                table: "NoteHint");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteLocation_AspNetUsers_UserId",
                table: "NoteLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_NotePerson_AspNetUsers_UserId",
                table: "NotePerson");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteQuest_AspNetUsers_UserId",
                table: "NoteQuest");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteStory_AspNetUsers_UserId",
                table: "NoteStory");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "NoteStory",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "NoteStory",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "NoteQuest",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "NoteQuest",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "NotePerson",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "NotePerson",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Affiliation",
                table: "NotePerson",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "NotePerson",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "NoteLocation",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "NoteLocation",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "NoteHint",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "NoteHint",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteHint_AspNetUsers_UserId",
                table: "NoteHint",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteLocation_AspNetUsers_UserId",
                table: "NoteLocation",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotePerson_AspNetUsers_UserId",
                table: "NotePerson",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteQuest_AspNetUsers_UserId",
                table: "NoteQuest",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteStory_AspNetUsers_UserId",
                table: "NoteStory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteHint_AspNetUsers_UserId",
                table: "NoteHint");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteLocation_AspNetUsers_UserId",
                table: "NoteLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_NotePerson_AspNetUsers_UserId",
                table: "NotePerson");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteQuest_AspNetUsers_UserId",
                table: "NoteQuest");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteStory_AspNetUsers_UserId",
                table: "NoteStory");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "NoteStory");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "NoteQuest");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "NotePerson");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "NoteLocation");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "NoteHint");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "NoteStory",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "NoteQuest",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "NotePerson",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "NotePerson",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Affiliation",
                table: "NotePerson",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "NoteLocation",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "NoteHint",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteHint_AspNetUsers_UserId",
                table: "NoteHint",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteLocation_AspNetUsers_UserId",
                table: "NoteLocation",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotePerson_AspNetUsers_UserId",
                table: "NotePerson",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteQuest_AspNetUsers_UserId",
                table: "NoteQuest",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteStory_AspNetUsers_UserId",
                table: "NoteStory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
