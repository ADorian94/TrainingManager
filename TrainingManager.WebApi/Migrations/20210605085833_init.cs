using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingManager.WebApi.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeightWorkouts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkoutGuid = table.Column<Guid>(nullable: false),
                    WorkoutName = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    WorkoutType = table.Column<int>(nullable: false),
                    WorkoutDate = table.Column<DateTime>(nullable: false),
                    TotalWeight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightWorkouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkoutId = table.Column<int>(nullable: false),
                    ImageSmall = table.Column<byte[]>(nullable: true),
                    ImageLarge = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeightExercises",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExerciseGuid = table.Column<Guid>(nullable: false),
                    ExerciseName = table.Column<string>(nullable: true),
                    TotalExerciseWeight = table.Column<double>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    WorkoutId = table.Column<int>(nullable: false),
                    WeightWorkoutId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeightExercises_WeightWorkouts_WeightWorkoutId",
                        column: x => x.WeightWorkoutId,
                        principalTable: "WeightWorkouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WeightRounds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoundGuid = table.Column<Guid>(nullable: false),
                    RoundNumber = table.Column<int>(nullable: false),
                    WeightOfExercise = table.Column<double>(nullable: false),
                    Reps = table.Column<int>(nullable: false),
                    ExerciseId = table.Column<int>(nullable: false),
                    WeightExerciseId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightRounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeightRounds_WeightExercises_WeightExerciseId",
                        column: x => x.WeightExerciseId,
                        principalTable: "WeightExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeightExercises_WeightWorkoutId",
                table: "WeightExercises",
                column: "WeightWorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightRounds_WeightExerciseId",
                table: "WeightRounds",
                column: "WeightExerciseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeightRounds");

            migrationBuilder.DropTable(
                name: "WorkoutImages");

            migrationBuilder.DropTable(
                name: "WeightExercises");

            migrationBuilder.DropTable(
                name: "WeightWorkouts");
        }
    }
}
