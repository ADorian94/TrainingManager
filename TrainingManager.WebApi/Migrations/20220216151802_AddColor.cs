using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingManager.WebApi.Migrations
{
    public partial class AddColor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Color",
                table: "WeightRounds",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Color",
                table: "WeightExercises",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "WeightRounds");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "WeightExercises");
        }
    }
}
