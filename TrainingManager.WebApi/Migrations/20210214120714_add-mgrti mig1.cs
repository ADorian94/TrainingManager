using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingManager.WebApi.Migrations
{
    public partial class addmgrtimig1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeightWorkouts",
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
                    table.PrimaryKey("PK_WeightWorkouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeightRounds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoundGuid = table.Column<Guid>(nullable: false),
                    RoundName = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    Reps = table.Column<int>(nullable: false),
                    WorkoutId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightRounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeightRounds_WeightWorkouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "WeightWorkouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeightDrills",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DrillGuid = table.Column<Guid>(nullable: false),
                    DrillName = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    WeightOfDrill = table.Column<double>(nullable: false),
                    DrillDate = table.Column<DateTime>(nullable: false),
                    Reps = table.Column<int>(nullable: false),
                    WorkoutId = table.Column<int>(nullable: false),
                    RoundId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightDrills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeightDrills_WeightRounds_RoundId",
                        column: x => x.RoundId,
                        principalTable: "WeightRounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeightDrills_WeightWorkouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "WeightWorkouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeightExercises",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExerciseGuid = table.Column<Guid>(nullable: false),
                    ExerciseName = table.Column<string>(nullable: true),
                    DrillId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeightExercises_WeightDrills_DrillId",
                        column: x => x.DrillId,
                        principalTable: "WeightDrills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeightDrills_RoundId",
                table: "WeightDrills",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightDrills_WorkoutId",
                table: "WeightDrills",
                column: "WorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightExercises_DrillId",
                table: "WeightExercises",
                column: "DrillId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightRounds_WorkoutId",
                table: "WeightRounds",
                column: "WorkoutId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeightExercises");

            migrationBuilder.DropTable(
                name: "WeightDrills");

            migrationBuilder.DropTable(
                name: "WeightRounds");

            migrationBuilder.DropTable(
                name: "WeightWorkouts");
        }
    }
}
