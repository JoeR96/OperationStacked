using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OperationStacked.Migrations
{
    /// <inheritdoc />
    public partial class PostGRES1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentStacks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartWeight = table.Column<decimal>(type: "numeric", nullable: false),
                    InitialIncrements = table.Column<string>(type: "text", nullable: false),
                    IncrementValue = table.Column<decimal>(type: "numeric", nullable: false),
                    IncrementCount = table.Column<decimal>(type: "numeric", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentStacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExerciseName = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    EquipmentType = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    CognitoUserId = table.Column<Guid>(type: "uuid", maxLength: 255, nullable: false),
                    CurrentWeek = table.Column<int>(type: "integer", nullable: false),
                    CurrentDay = table.Column<int>(type: "integer", nullable: false),
                    WorkoutDaysInWeek = table.Column<int>(type: "integer", nullable: false),
                    WorkoutWeeks = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workouts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkoutName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "A2SHypertrophyExercise",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TrainingMax = table.Column<decimal>(type: "numeric", nullable: false),
                    PrimaryLift = table.Column<bool>(type: "boolean", nullable: false),
                    Block = table.Column<int>(type: "integer", nullable: false),
                    AmrapRepTarget = table.Column<int>(type: "integer", nullable: false),
                    AmrapRepResult = table.Column<int>(type: "integer", nullable: false),
                    Week = table.Column<int>(type: "integer", nullable: false),
                    Intensity = table.Column<decimal>(type: "numeric", nullable: false),
                    Sets = table.Column<int>(type: "integer", nullable: false),
                    RepsPerSet = table.Column<int>(type: "integer", nullable: false),
                    RoundingValue = table.Column<decimal>(type: "numeric", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "ExerciseHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedSets = table.Column<int>(type: "integer", nullable: false),
                    CompletedReps = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateExerciseId = table.Column<Guid>(type: "uuid", nullable: true),
                    WorkingWeight = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseHistory_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutExercises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkoutId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Template = table.Column<int>(type: "integer", nullable: false),
                    LiftDay = table.Column<int>(type: "integer", nullable: false),
                    LiftOrder = table.Column<int>(type: "integer", nullable: false),
                    Completed = table.Column<bool>(type: "boolean", nullable: false),
                    RestTimer = table.Column<int>(type: "integer", nullable: false),
                    MinimumReps = table.Column<int>(type: "integer", nullable: false),
                    MaximumReps = table.Column<int>(type: "integer", nullable: false),
                    Sets = table.Column<int>(type: "integer", nullable: false),
                    WeightProgression = table.Column<decimal>(type: "numeric", nullable: false),
                    AttemptsBeforeDeload = table.Column<int>(type: "integer", nullable: false),
                    EquipmentStackId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutExercises_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LinearProgressionExercise",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkoutExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentAttempt = table.Column<int>(type: "integer", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: false),
                    LiftWeek = table.Column<int>(type: "integer", nullable: false),
                    WorkingWeight = table.Column<decimal>(type: "numeric", nullable: false),
                    WeightIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinearProgressionExercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinearProgressionExercise_WorkoutExercises_WorkoutExerciseId",
                        column: x => x.WorkoutExerciseId,
                        principalTable: "WorkoutExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseHistory_ExerciseId",
                table: "ExerciseHistory",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_LinearProgressionExercise_WorkoutExerciseId",
                table: "LinearProgressionExercise",
                column: "WorkoutExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExercises_ExerciseId",
                table: "WorkoutExercises",
                column: "ExerciseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "A2SHypertrophyExercise");

            migrationBuilder.DropTable(
                name: "EquipmentStacks");

            migrationBuilder.DropTable(
                name: "ExerciseHistory");

            migrationBuilder.DropTable(
                name: "LinearProgressionExercise");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Workouts");

            migrationBuilder.DropTable(
                name: "WorkoutExercises");

            migrationBuilder.DropTable(
                name: "Exercises");
        }
    }
}
