using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingManager.WebApi.Migrations
{
    public partial class userhandling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerUserName",
                table: "WeightWorkouts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerUserName",
                table: "WeightExercises",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerUserName",
                table: "WeightActivities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerUserName",
                table: "WeightWorkouts");

            migrationBuilder.DropColumn(
                name: "OwnerUserName",
                table: "WeightExercises");

            migrationBuilder.DropColumn(
                name: "OwnerUserName",
                table: "WeightActivities");
        }
    }
}
