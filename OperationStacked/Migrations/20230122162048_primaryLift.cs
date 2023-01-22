using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OperationStacked.Migrations
{
    public partial class primaryLift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "LinearProgressionExercise");

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Exercises",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<decimal>(
                name: "RoundingValue",
                table: "A2SHypertrophyExercise",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "RoundingValue",
                table: "A2SHypertrophyExercise");

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "LinearProgressionExercise",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");
        }
    }
}
