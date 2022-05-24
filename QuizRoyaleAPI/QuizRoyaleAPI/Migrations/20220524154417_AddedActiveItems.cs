using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizRoyaleAPI.Migrations
{
    public partial class AddedActiveItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemPlayer");

            migrationBuilder.DropColumn(
                name: "Border",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Players");

            migrationBuilder.CreateTable(
                name: "AcquiredItem",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    Equipped = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcquiredItem", x => new { x.ItemId, x.PlayerId });
                    table.ForeignKey(
                        name: "FK_AcquiredItem_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AcquiredItem_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AcquiredItem_PlayerId",
                table: "AcquiredItem",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcquiredItem");

            migrationBuilder.AddColumn<string>(
                name: "Border",
                table: "Players",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "Players",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Players",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ItemPlayer",
                columns: table => new
                {
                    AcquiredItemsId = table.Column<int>(type: "int", nullable: false),
                    PlayersWhoAcquiredId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPlayer", x => new { x.AcquiredItemsId, x.PlayersWhoAcquiredId });
                    table.ForeignKey(
                        name: "FK_ItemPlayer_Items_AcquiredItemsId",
                        column: x => x.AcquiredItemsId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemPlayer_Players_PlayersWhoAcquiredId",
                        column: x => x.PlayersWhoAcquiredId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPlayer_PlayersWhoAcquiredId",
                table: "ItemPlayer",
                column: "PlayersWhoAcquiredId");
        }
    }
}
