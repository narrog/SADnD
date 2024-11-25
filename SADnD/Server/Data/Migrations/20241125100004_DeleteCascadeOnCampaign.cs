using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SADnD.Server.Migrations
{
    /// <inheritdoc />
    public partial class DeleteCascadeOnCampaign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteHint_Campaigns_CampaignId",
                table: "NoteHint");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteLocation_Campaigns_CampaignId",
                table: "NoteLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_NotePerson_Campaigns_CampaignId",
                table: "NotePerson");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteQuest_Campaigns_CampaignId",
                table: "NoteQuest");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteStory_Campaigns_CampaignId",
                table: "NoteStory");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteHint_Campaigns_CampaignId",
                table: "NoteHint",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteLocation_Campaigns_CampaignId",
                table: "NoteLocation",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotePerson_Campaigns_CampaignId",
                table: "NotePerson",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteQuest_Campaigns_CampaignId",
                table: "NoteQuest",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteStory_Campaigns_CampaignId",
                table: "NoteStory",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteHint_Campaigns_CampaignId",
                table: "NoteHint");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteLocation_Campaigns_CampaignId",
                table: "NoteLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_NotePerson_Campaigns_CampaignId",
                table: "NotePerson");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteQuest_Campaigns_CampaignId",
                table: "NoteQuest");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteStory_Campaigns_CampaignId",
                table: "NoteStory");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteHint_Campaigns_CampaignId",
                table: "NoteHint",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteLocation_Campaigns_CampaignId",
                table: "NoteLocation",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotePerson_Campaigns_CampaignId",
                table: "NotePerson",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteQuest_Campaigns_CampaignId",
                table: "NoteQuest",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteStory_Campaigns_CampaignId",
                table: "NoteStory",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id");
        }
    }
}
