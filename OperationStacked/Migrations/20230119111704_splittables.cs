using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OperationStacked.Migrations
{
    public partial class splittables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmrapRepResult",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "AmrapRepTarget",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "AttemptsBeforeDeload",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "AuxillaryLift",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "Block",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "CurrentAttempt",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "CurrentSets",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "Intensity",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "MaximumReps",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "MinimumReps",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "PrimaryExercise",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "RepsPerSet",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "Sets",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "StartingSets",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "StartingWeight",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "TargetSets",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "TrainingMax",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "Week",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "WeightIndex",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "WeightProgression",
                table: "Exercises");

            migrationBuilder.CreateTable(
                name: "A2SHypertrophyExercise",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TrainingMax = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    AuxillaryLift = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Block = table.Column<int>(type: "int", nullable: false),
                    AmrapRepTarget = table.Column<int>(type: "int", nullable: false),
                    AmrapRepResult = table.Column<int>(type: "int", nullable: false),
                    Week = table.Column<int>(type: "int", nullable: false),
                    Intensity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Sets = table.Column<int>(type: "int", nullable: false),
                    RepsPerSet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_A2SHypertrophyExercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_A2SHypertrophyExercise_Exercises_Id",
                        column: x => x.Id,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LinearProgressionExercise",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MinimumReps = table.Column<int>(type: "int", nullable: false),
                    MaximumReps = table.Column<int>(type: "int", nullable: false),
                    TargetSets = table.Column<int>(type: "int", nullable: false),
                    StartingSets = table.Column<int>(type: "int", nullable: false),
                    CurrentSets = table.Column<int>(type: "int", nullable: false),
                    WeightIndex = table.Column<int>(type: "int", nullable: false),
                    PrimaryExercise = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    StartingWeight = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    WeightProgression = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    AttemptsBeforeDeload = table.Column<int>(type: "int", nullable: false),
                    CurrentAttempt = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinearProgressionExercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinearProgressionExercise_Exercises_Id",
                        column: x => x.Id,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "A2SHypertrophyExercise");

            migrationBuilder.DropTable(
                name: "LinearProgressionExercise");

            migrationBuilder.AddColumn<int>(
                name: "AmrapRepResult",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AmrapRepTarget",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AttemptsBeforeDeload",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AuxillaryLift",
                table: "Exercises",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Block",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentAttempt",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentSets",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Exercises",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "Intensity",
                table: "Exercises",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaximumReps",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinimumReps",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Exercises",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<bool>(
                name: "PrimaryExercise",
                table: "Exercises",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepsPerSet",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sets",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StartingSets",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StartingWeight",
                table: "Exercises",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetSets",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TrainingMax",
                table: "Exercises",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Week",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeightIndex",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WeightProgression",
                table: "Exercises",
                type: "decimal(65,30)",
                nullable: true);
        }
    }
}
