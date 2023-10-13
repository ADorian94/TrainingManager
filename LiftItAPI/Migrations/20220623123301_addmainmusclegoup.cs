using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingManager.WebApi.Migrations
{
    public partial class addmainmusclegoup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MainMuscleGroup",
                table: "WeightActivities",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainMuscleGroup",
                table: "WeightActivities");
        }
    }
}
