using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizRoyaleAPI.Migrations
{
    public partial class AddDivisionTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Division_Ranks_RankId",
                table: "Division");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Division",
                table: "Division");

            migrationBuilder.RenameTable(
                name: "Division",
                newName: "Divisions");

            migrationBuilder.RenameIndex(
                name: "IX_Division_RankId",
                table: "Divisions",
                newName: "IX_Divisions_RankId");

            migrationBuilder.AlterColumn<int>(
                name: "RankId",
                table: "Divisions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Divisions",
                table: "Divisions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Divisions_Ranks_RankId",
                table: "Divisions",
                column: "RankId",
                principalTable: "Ranks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Divisions_Ranks_RankId",
                table: "Divisions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Divisions",
                table: "Divisions");

            migrationBuilder.RenameTable(
                name: "Divisions",
                newName: "Division");

            migrationBuilder.RenameIndex(
                name: "IX_Divisions_RankId",
                table: "Division",
                newName: "IX_Division_RankId");

            migrationBuilder.AlterColumn<int>(
                name: "RankId",
                table: "Division",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Division",
                table: "Division",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Division_Ranks_RankId",
                table: "Division",
                column: "RankId",
                principalTable: "Ranks",
                principalColumn: "Id");
        }
    }
}
