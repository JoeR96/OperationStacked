using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OperationStacked.Migrations
{
    public partial class baserefacctor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EquipmentStackId",
                table: "LinearProgressionExercise");

            migrationBuilder.DropColumn(
                name: "WeightIndex",
                table: "LinearProgressionExercise");

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

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentStackId",
                table: "Exercises",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "WeightIndex",
                table: "Exercises",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EquipmentStackId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "WeightIndex",
                table: "Exercises");

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

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentStackId",
                table: "LinearProgressionExercise",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "WeightIndex",
                table: "LinearProgressionExercise",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
