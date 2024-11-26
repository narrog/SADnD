using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SADnD.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "NoteSequence");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "InventoryItems",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "NoteHint",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"NoteSequence\"')"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CampaignId = table.Column<string>(type: "text", nullable: true),
                    CharacterId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteHint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NoteHint_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NoteHint_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NoteHint_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NoteLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"NoteSequence\"')"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CampaignId = table.Column<string>(type: "text", nullable: true),
                    CharacterId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NoteLocation_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NoteLocation_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NoteLocation_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NoteMentions",
                columns: table => new
                {
                    NoteMentionsId = table.Column<int>(type: "integer", nullable: false),
                    NotesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteMentions", x => new { x.NoteMentionsId, x.NotesId });
                });

            migrationBuilder.CreateTable(
                name: "NotePerson",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"NoteSequence\"')"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CampaignId = table.Column<string>(type: "text", nullable: true),
                    CharacterId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: false),
                    Affiliation = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotePerson", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotePerson_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NotePerson_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NotePerson_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NoteQuest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"NoteSequence\"')"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CampaignId = table.Column<string>(type: "text", nullable: true),
                    CharacterId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteQuest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NoteQuest_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NoteQuest_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NoteQuest_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NoteStory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"NoteSequence\"')"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CampaignId = table.Column<string>(type: "text", nullable: true),
                    CharacterId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteStory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NoteStory_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NoteStory_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NoteStory_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NoteHint_CampaignId",
                table: "NoteHint",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteHint_CharacterId",
                table: "NoteHint",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteHint_UserId",
                table: "NoteHint",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteLocation_CampaignId",
                table: "NoteLocation",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteLocation_CharacterId",
                table: "NoteLocation",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteLocation_UserId",
                table: "NoteLocation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteMentions_NotesId",
                table: "NoteMentions",
                column: "NotesId");

            migrationBuilder.CreateIndex(
                name: "IX_NotePerson_CampaignId",
                table: "NotePerson",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_NotePerson_CharacterId",
                table: "NotePerson",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_NotePerson_UserId",
                table: "NotePerson",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteQuest_CampaignId",
                table: "NoteQuest",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteQuest_CharacterId",
                table: "NoteQuest",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteQuest_UserId",
                table: "NoteQuest",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteStory_CampaignId",
                table: "NoteStory",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteStory_CharacterId",
                table: "NoteStory",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteStory_UserId",
                table: "NoteStory",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoteHint");

            migrationBuilder.DropTable(
                name: "NoteLocation");

            migrationBuilder.DropTable(
                name: "NoteMentions");

            migrationBuilder.DropTable(
                name: "NotePerson");

            migrationBuilder.DropTable(
                name: "NoteQuest");

            migrationBuilder.DropTable(
                name: "NoteStory");

            migrationBuilder.DropSequence(
                name: "NoteSequence");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "InventoryItems",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);
        }
    }
}
