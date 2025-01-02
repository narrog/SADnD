using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SADnD.Server.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceCampaignId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Campaigns_Id",
                table: "Campaigns");

            migrationBuilder.AddColumn<string>(
                name: "JoinCode",
                table: "Campaigns",
                type: "text",
                nullable: false,
                defaultValue: "");

            // Copy Id to new Column
            migrationBuilder.Sql(@"
                UPDATE ""Campaigns""
                SET ""JoinCode"" = ""Id""
            ");

            // Temporär Fremdschlüssel deaktivieren
            migrationBuilder.Sql("ALTER TABLE \"Appointments\" DROP CONSTRAINT \"FK_Appointments_Campaigns_CampaignId\";");
            migrationBuilder.Sql("ALTER TABLE \"Characters\" DROP CONSTRAINT \"FK_Characters_Campaigns_CampaignId\";");
            migrationBuilder.Sql("ALTER TABLE \"JoinRequests\" DROP CONSTRAINT \"FK_JoinRequests_Campaigns_CampaignId\";");
            migrationBuilder.Sql("ALTER TABLE \"NoteStory\" DROP CONSTRAINT \"FK_NoteStory_Campaigns_CampaignId\";");
            migrationBuilder.Sql("ALTER TABLE \"NotePerson\" DROP CONSTRAINT \"FK_NotePerson_Campaigns_CampaignId\";");
            migrationBuilder.Sql("ALTER TABLE \"NoteLocation\" DROP CONSTRAINT \"FK_NoteLocation_Campaigns_CampaignId\";");
            migrationBuilder.Sql("ALTER TABLE \"NoteQuest\" DROP CONSTRAINT \"FK_NoteQuest_Campaigns_CampaignId\";");
            migrationBuilder.Sql("ALTER TABLE \"NoteHint\" DROP CONSTRAINT \"FK_NoteHint_Campaigns_CampaignId\";");
            migrationBuilder.Sql("ALTER TABLE \"CampaignDungeonMasters\" DROP CONSTRAINT \"FK_CampaignDungeonMasters_Campaigns_DungeonMasterCampaignsId\";");
            migrationBuilder.Sql("ALTER TABLE \"CampaignPlayers\" DROP CONSTRAINT \"FK_CampaignPlayers_Campaigns_PlayerCampaignsId\";");

            // Generate Id's
            migrationBuilder.Sql(@"
                UPDATE ""Campaigns""
                SET ""Id"" = subquery.new_id::text
                FROM (
                    SELECT ""Id"", ROW_NUMBER() OVER (ORDER BY ""Id"") AS new_id
                    FROM ""Campaigns""
                ) AS subquery
                WHERE ""Campaigns"".""Id"" = subquery.""Id"";
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""Campaigns"" 
                ALTER COLUMN ""Id"" TYPE integer 
                USING ""Id""::integer;
            ");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Campaigns",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            //// Generate incremental Id's
            //migrationBuilder.Sql(@"
            //    UPDATE ""Campaigns""
            //    SET ""Id"" = nextval(pg_get_serial_sequence('""Campaigns""','Id'));
            //");

            // Replace foreign keys
            migrationBuilder.Sql(@"
                Update ""Appointments""
                Set ""CampaignId"" = (SELECT ""Id"" FROM ""Campaigns"" WHERE ""Campaigns"".""JoinCode"" = ""Appointments"".""CampaignId"")
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""Appointments"" 
                ALTER COLUMN ""CampaignId"" TYPE integer 
                USING ""CampaignId""::integer;
            ");

            //migrationBuilder.AlterColumn<int>(
            //    name: "CampaignId",
            //    table: "Appointments",
            //    type: "integer",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "text");

            // Replace foreign keys
            migrationBuilder.Sql(@"
                Update ""Characters""
                Set ""CampaignId"" = (SELECT ""Id"" FROM ""Campaigns"" WHERE ""Campaigns"".""JoinCode"" = ""Characters"".""CampaignId"")
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""Characters"" 
                ALTER COLUMN ""CampaignId"" TYPE integer 
                USING ""CampaignId""::integer;
            ");

            //migrationBuilder.AlterColumn<int>(
            //    name: "CampaignId",
            //    table: "Characters",
            //    type: "integer",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "text",
            //    oldNullable: true);

            // Replace foreign keys
            migrationBuilder.Sql(@"
                Update ""JoinRequests""
                Set ""CampaignId"" = (SELECT ""Id"" FROM ""Campaigns"" WHERE ""Campaigns"".""JoinCode"" = ""JoinRequests"".""CampaignId"")
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""JoinRequests"" 
                ALTER COLUMN ""CampaignId"" TYPE integer 
                USING ""CampaignId""::integer;
            ");

            //migrationBuilder.AlterColumn<int>(
            //    name: "CampaignId",
            //    table: "JoinRequests",
            //    type: "integer",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "character varying(8)",
            //    oldMaxLength: 8);

            migrationBuilder.AddColumn<string>(
                name: "JoinCode",
                table: "JoinRequests",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            // Fill in JoinCodes in Requests
            migrationBuilder.Sql(@"
                Update ""JoinRequests""
                Set ""JoinCode"" = (SELECT ""JoinCode"" FROM ""Campaigns"" WHERE ""Campaigns"".""Id"" = ""JoinRequests"".""CampaignId"")
            ");

            // Replace foreign keys
            migrationBuilder.Sql(@"
                Update ""NoteStory""
                Set ""CampaignId"" = (SELECT ""Id"" FROM ""Campaigns"" WHERE ""Campaigns"".""JoinCode"" = ""NoteStory"".""CampaignId"")
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""NoteStory"" 
                ALTER COLUMN ""CampaignId"" TYPE integer 
                USING ""CampaignId""::integer;
            ");

            //migrationBuilder.AlterColumn<int>(
            //    name: "CampaignId",
            //    table: "NoteStory",
            //    type: "integer",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "text",
            //    oldNullable: true);

            // Replace foreign keys
            migrationBuilder.Sql(@"
                Update ""NotePerson""
                Set ""CampaignId"" = (SELECT ""Id"" FROM ""Campaigns"" WHERE ""Campaigns"".""JoinCode"" = ""NotePerson"".""CampaignId"")
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""NotePerson"" 
                ALTER COLUMN ""CampaignId"" TYPE integer 
                USING ""CampaignId""::integer;
            ");

            //migrationBuilder.AlterColumn<int>(
            //    name: "CampaignId",
            //    table: "NotePerson",
            //    type: "integer",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "text",
            //    oldNullable: true);

            // Replace foreign keys
            migrationBuilder.Sql(@"
                Update ""NoteLocation""
                Set ""CampaignId"" = (SELECT ""Id"" FROM ""Campaigns"" WHERE ""Campaigns"".""JoinCode"" = ""NoteLocation"".""CampaignId"")
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""NoteLocation"" 
                ALTER COLUMN ""CampaignId"" TYPE integer 
                USING ""CampaignId""::integer;
            ");

            //migrationBuilder.AlterColumn<int>(
            //    name: "CampaignId",
            //    table: "NoteLocation",
            //    type: "integer",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "text",
            //    oldNullable: true);

            // Replace foreign keys
            migrationBuilder.Sql(@"
                Update ""NoteQuest""
                Set ""CampaignId"" = (SELECT ""Id"" FROM ""Campaigns"" WHERE ""Campaigns"".""JoinCode"" = ""NoteQuest"".""CampaignId"")
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""NoteQuest"" 
                ALTER COLUMN ""CampaignId"" TYPE integer 
                USING ""CampaignId""::integer;
            ");

            //migrationBuilder.AlterColumn<int>(
            //    name: "CampaignId",
            //    table: "NoteQuest",
            //    type: "integer",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "text",
            //    oldNullable: true);

            // Replace foreign keys
            migrationBuilder.Sql(@"
                Update ""NoteHint""
                Set ""CampaignId"" = (SELECT ""Id"" FROM ""Campaigns"" WHERE ""Campaigns"".""JoinCode"" = ""NoteHint"".""CampaignId"")
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""NoteHint"" 
                ALTER COLUMN ""CampaignId"" TYPE integer 
                USING ""CampaignId""::integer;
            ");

            //migrationBuilder.AlterColumn<int>(
            //    name: "CampaignId",
            //    table: "NoteHint",
            //    type: "integer",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "text",
            //    oldNullable: true);

            //Replace Foreign Keys in m:n-Tables
            migrationBuilder.Sql(@"
                Update ""CampaignPlayers""
                Set ""PlayerCampaignsId"" = (SELECT ""Id"" FROM ""Campaigns"" WHERE ""Campaigns"".""JoinCode"" = ""CampaignPlayers"".""PlayerCampaignsId"")
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""CampaignPlayers"" 
                ALTER COLUMN ""PlayerCampaignsId"" TYPE integer 
                USING ""PlayerCampaignsId""::integer;
            ");

            //migrationBuilder.AlterColumn<int>(
            //    name: "PlayerCampaignsId",
            //    table: "CampaignPlayers",
            //    type: "integer",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "text");

            //Replace Foreign Keys in m:n-Tables
            migrationBuilder.Sql(@"
                Update ""CampaignDungeonMasters""
                Set ""DungeonMasterCampaignsId"" = (SELECT ""Id"" FROM ""Campaigns"" WHERE ""Campaigns"".""JoinCode"" = ""CampaignDungeonMasters"".""DungeonMasterCampaignsId"")
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""CampaignDungeonMasters"" 
                ALTER COLUMN ""DungeonMasterCampaignsId"" TYPE integer 
                USING ""DungeonMasterCampaignsId""::integer;
            ");

            //migrationBuilder.AlterColumn<int>(
            //    name: "DungeonMasterCampaignsId",
            //    table: "CampaignDungeonMasters",
            //    type: "integer",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "text");

            // Fremdschlüssel reaktivieren
            migrationBuilder.Sql("ALTER TABLE \"Appointments\" ADD CONSTRAINT \"FK_Appointments_Campaigns_CampaignId\" FOREIGN KEY (\"CampaignId\") REFERENCES \"Campaigns\"(\"Id\") ON DELETE CASCADE;");
            migrationBuilder.Sql("ALTER TABLE \"Characters\" ADD CONSTRAINT \"FK_Characters_Campaigns_CampaignId\" FOREIGN KEY (\"CampaignId\") REFERENCES \"Campaigns\"(\"Id\") ON DELETE NO ACTION;");
            migrationBuilder.Sql("ALTER TABLE \"JoinRequests\" ADD CONSTRAINT \"FK_JoinRequests_Campaigns_CampaignId\" FOREIGN KEY (\"CampaignId\") REFERENCES \"Campaigns\"(\"Id\") ON DELETE CASCADE;");
            migrationBuilder.Sql("ALTER TABLE \"NoteStory\" ADD CONSTRAINT \"FK_NoteStory_Campaigns_CampaignId\" FOREIGN KEY (\"CampaignId\") REFERENCES \"Campaigns\"(\"Id\") ON DELETE CASCADE;");
            migrationBuilder.Sql("ALTER TABLE \"NotePerson\" ADD CONSTRAINT \"FK_NotePerson_Campaigns_CampaignId\" FOREIGN KEY (\"CampaignId\") REFERENCES \"Campaigns\"(\"Id\") ON DELETE CASCADE;");
            migrationBuilder.Sql("ALTER TABLE \"NoteLocation\" ADD CONSTRAINT \"FK_NoteLocation_Campaigns_CampaignId\" FOREIGN KEY (\"CampaignId\") REFERENCES \"Campaigns\"(\"Id\") ON DELETE CASCADE;");
            migrationBuilder.Sql("ALTER TABLE \"NoteQuest\" ADD CONSTRAINT \"FK_NoteQuest_Campaigns_CampaignId\" FOREIGN KEY (\"CampaignId\") REFERENCES \"Campaigns\"(\"Id\") ON DELETE CASCADE;");
            migrationBuilder.Sql("ALTER TABLE \"NoteHint\" ADD CONSTRAINT \"FK_NoteHint_Campaigns_CampaignId\" FOREIGN KEY (\"CampaignId\") REFERENCES \"Campaigns\"(\"Id\") ON DELETE CASCADE;");
            migrationBuilder.Sql("ALTER TABLE \"CampaignDungeonMasters\" ADD CONSTRAINT \"FK_CampaignDungeonMasters_Campaigns_DungeonMasterCampaignsId\" FOREIGN KEY (\"DungeonMasterCampaignsId\") REFERENCES \"Campaigns\"(\"Id\") ON DELETE CASCADE;");
            migrationBuilder.Sql("ALTER TABLE \"CampaignPlayers\" ADD CONSTRAINT \"FK_CampaignPlayers_Campaigns_PlayerCampaignsId\" FOREIGN KEY (\"PlayerCampaignsId\") REFERENCES \"Campaigns\"(\"Id\") ON DELETE CASCADE;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JoinCode",
                table: "JoinRequests");

            migrationBuilder.DropColumn(
                name: "JoinCode",
                table: "Campaigns");

            migrationBuilder.AlterColumn<string>(
                name: "CampaignId",
                table: "NoteStory",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CampaignId",
                table: "NoteQuest",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CampaignId",
                table: "NotePerson",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CampaignId",
                table: "NoteLocation",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CampaignId",
                table: "NoteHint",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CampaignId",
                table: "JoinRequests",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "CampaignId",
                table: "Characters",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Campaigns",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "PlayerCampaignsId",
                table: "CampaignPlayers",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "DungeonMasterCampaignsId",
                table: "CampaignDungeonMasters",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "CampaignId",
                table: "Appointments",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_Id",
                table: "Campaigns",
                column: "Id",
                unique: true);
        }
    }
}
