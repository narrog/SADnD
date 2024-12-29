using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SADnD.Server.Migrations
{
    /// <inheritdoc />
    public partial class RenameUserIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampaignDungeonMasters_AspNetUsers_DungeonMastersId",
                table: "CampaignDungeonMasters");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignPlayers_AspNetUsers_PlayersId",
                table: "CampaignPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterAccess_AspNetUsers_UserAccessId",
                table: "CharacterAccess");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CampaignPlayers",
                table: "CampaignPlayers");

            migrationBuilder.DropIndex(
                name: "IX_CampaignPlayers_PlayersId",
                table: "CampaignPlayers");

            migrationBuilder.RenameColumn(
                name: "UserAccessId",
                table: "CharacterAccess",
                newName: "EFUserAccessId");

            migrationBuilder.RenameIndex(
                name: "IX_CharacterAccess_UserAccessId",
                table: "CharacterAccess",
                newName: "IX_CharacterAccess_EFUserAccessId");

            migrationBuilder.RenameColumn(
                name: "PlayersId",
                table: "CampaignPlayers",
                newName: "EFPlayersId");

            migrationBuilder.RenameColumn(
                name: "DungeonMastersId",
                table: "CampaignDungeonMasters",
                newName: "EFDungeonMastersId");

            migrationBuilder.RenameIndex(
                name: "IX_CampaignDungeonMasters_DungeonMastersId",
                table: "CampaignDungeonMasters",
                newName: "IX_CampaignDungeonMasters_EFDungeonMastersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CampaignPlayers",
                table: "CampaignPlayers",
                columns: new[] { "EFPlayersId", "PlayerCampaignsId" });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignPlayers_PlayerCampaignsId",
                table: "CampaignPlayers",
                column: "PlayerCampaignsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignDungeonMasters_AspNetUsers_EFDungeonMastersId",
                table: "CampaignDungeonMasters",
                column: "EFDungeonMastersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignPlayers_AspNetUsers_EFPlayersId",
                table: "CampaignPlayers",
                column: "EFPlayersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterAccess_AspNetUsers_EFUserAccessId",
                table: "CharacterAccess",
                column: "EFUserAccessId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampaignDungeonMasters_AspNetUsers_EFDungeonMastersId",
                table: "CampaignDungeonMasters");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignPlayers_AspNetUsers_EFPlayersId",
                table: "CampaignPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterAccess_AspNetUsers_EFUserAccessId",
                table: "CharacterAccess");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CampaignPlayers",
                table: "CampaignPlayers");

            migrationBuilder.DropIndex(
                name: "IX_CampaignPlayers_PlayerCampaignsId",
                table: "CampaignPlayers");

            migrationBuilder.RenameColumn(
                name: "EFUserAccessId",
                table: "CharacterAccess",
                newName: "UserAccessId");

            migrationBuilder.RenameIndex(
                name: "IX_CharacterAccess_EFUserAccessId",
                table: "CharacterAccess",
                newName: "IX_CharacterAccess_UserAccessId");

            migrationBuilder.RenameColumn(
                name: "EFPlayersId",
                table: "CampaignPlayers",
                newName: "PlayersId");

            migrationBuilder.RenameColumn(
                name: "EFDungeonMastersId",
                table: "CampaignDungeonMasters",
                newName: "DungeonMastersId");

            migrationBuilder.RenameIndex(
                name: "IX_CampaignDungeonMasters_EFDungeonMastersId",
                table: "CampaignDungeonMasters",
                newName: "IX_CampaignDungeonMasters_DungeonMastersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CampaignPlayers",
                table: "CampaignPlayers",
                columns: new[] { "PlayerCampaignsId", "PlayersId" });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignPlayers_PlayersId",
                table: "CampaignPlayers",
                column: "PlayersId");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignDungeonMasters_AspNetUsers_DungeonMastersId",
                table: "CampaignDungeonMasters",
                column: "DungeonMastersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignPlayers_AspNetUsers_PlayersId",
                table: "CampaignPlayers",
                column: "PlayersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterAccess_AspNetUsers_UserAccessId",
                table: "CharacterAccess",
                column: "UserAccessId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
