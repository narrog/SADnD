using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SADnD.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_AspNetUsers_UserId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Characters_CharacterId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_CharacterId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "Classes");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Characters",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: true),
                    CampaignId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItems_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    InventoryItemId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventories_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventories_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_CharacterId",
                table: "Inventories",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_InventoryItemId",
                table: "Inventories",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_CampaignId",
                table: "InventoryItems",
                column: "CampaignId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_AspNetUsers_UserId",
                table: "Characters",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_AspNetUsers_UserId",
                table: "Characters");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.AddColumn<int>(
                name: "CharacterId",
                table: "Classes",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Characters",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CharacterId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CharacterId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CharacterId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CharacterId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CharacterId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 6,
                column: "CharacterId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 7,
                column: "CharacterId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 8,
                column: "CharacterId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 9,
                column: "CharacterId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 10,
                column: "CharacterId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 11,
                column: "CharacterId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 12,
                column: "CharacterId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CharacterId",
                table: "Classes",
                column: "CharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_AspNetUsers_UserId",
                table: "Characters",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Characters_CharacterId",
                table: "Classes",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id");
        }
    }
}
