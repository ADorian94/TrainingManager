using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingManager.WebApi.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeightWorkout",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkoutName = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    WorkoutType = table.Column<int>(nullable: false),
                    WorkoutDate = table.Column<DateTime>(nullable: false),
                    TotalWeight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightWorkout", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeightExercise",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExerciseName = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    WeightOfExercise = table.Column<double>(nullable: false),
                    ExerciseDate = table.Column<DateTime>(nullable: false),
                    Reps = table.Column<int>(nullable: false),
                    WeightWorkoutId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightExercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeightExercise_WeightWorkout_WeightWorkoutId",
                        column: x => x.WeightWorkoutId,
                        principalTable: "WeightWorkout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeightExercise_WeightWorkoutId",
                table: "WeightExercise",
                column: "WeightWorkoutId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeightExercise");

            migrationBuilder.DropTable(
                name: "WeightWorkout");
        }
    }
}
