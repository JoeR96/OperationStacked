using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OperationStacked.Migrations
{
    public partial class hello12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseHistory_LinearProgressionExercise_TemplateExerciseId",
                table: "ExerciseHistory");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseHistory_TemplateExerciseId",
                table: "ExerciseHistory");

            migrationBuilder.AlterColumn<Guid>(
                name: "CognitoUserId",
                table: "Users",
                type: "char(255)",
                maxLength: 255,
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "char(255)",
                oldMaxLength: 255)
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CognitoUserId",
                table: "Users",
                type: "char(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(255)",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseHistory_TemplateExerciseId",
                table: "ExerciseHistory",
                column: "TemplateExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseHistory_LinearProgressionExercise_TemplateExerciseId",
                table: "ExerciseHistory",
                column: "TemplateExerciseId",
                principalTable: "LinearProgressionExercise",
                principalColumn: "Id");
        }
    }
}
