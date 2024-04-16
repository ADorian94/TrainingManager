using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingManager.WebApi.Migrations
{
    public partial class PersonalRecords2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonalRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PersonalRecordGuid = table.Column<Guid>(nullable: false),
                    ActivityId = table.Column<int>(nullable: false),
                    PersonalRecordDate = table.Column<DateTime>(nullable: false),
                    WeightOfPersonalRecord = table.Column<double>(nullable: false),
                    RepsOfPersonalRecord = table.Column<int>(nullable: false),
                    WorkoutId = table.Column<int>(nullable: false),
                    OwnerUserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalRecords", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonalRecords");
        }
    }
}
