using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizRoyaleAPI.Migrations
{
    public partial class AddedMastery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Items",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CategoryMastery",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    AmountOfQuestions = table.Column<int>(type: "int", nullable: false),
                    QuestionsRight = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryMastery", x => new { x.CategoryId, x.PlayerId });
                    table.ForeignKey(
                        name: "FK_CategoryMastery_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryMastery_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryMastery_PlayerId",
                table: "CategoryMastery",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryMastery");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Items");
        }
    }
}
