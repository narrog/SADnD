using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SADnD.Server.Migrations
{
    /// <inheritdoc />
    public partial class ChangedItemLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Campaigns_CampaignId",
                table: "InventoryItems");

            migrationBuilder.RenameColumn(
                name: "CampaignId",
                table: "InventoryItems",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryItems_CampaignId",
                table: "InventoryItems",
                newName: "IX_InventoryItems_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "CampaignId",
                table: "JoinRequests",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_AspNetUsers_UserId",
                table: "InventoryItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_AspNetUsers_UserId",
                table: "InventoryItems");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "InventoryItems",
                newName: "CampaignId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryItems_UserId",
                table: "InventoryItems",
                newName: "IX_InventoryItems_CampaignId");

            migrationBuilder.AlterColumn<string>(
                name: "CampaignId",
                table: "JoinRequests",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Campaigns_CampaignId",
                table: "InventoryItems",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
