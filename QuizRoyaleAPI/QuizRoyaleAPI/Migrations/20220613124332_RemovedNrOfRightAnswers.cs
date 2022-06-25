using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizRoyaleAPI.Migrations
{
    public partial class RemovedNrOfRightAnswers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RightAnswers",
                table: "Result");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RightAnswers",
                table: "Result",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
