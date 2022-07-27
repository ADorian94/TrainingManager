using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingManager.WebApi.Migrations
{
    public partial class IsWatched : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsWatched",
                table: "WeightActivities",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWatched",
                table: "WeightActivities");
        }
    }
}
