using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OperationStacked.Migrations
{
    public partial class new88 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<Guid>(
                    name: "UserId",
                    table: "Exercises",
                    type: "char(36)",
                    nullable: false,
                    collation: "ascii_general_ci",
                    oldClrType: typeof(string),
                    oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinearProgressionExercise_EquipmentStacks_EquipmentStackId",
                table: "LinearProgressionExercise");

            migrationBuilder.DropTable(
                name: "EquipmentStacks");

            migrationBuilder.DropIndex(
                name: "IX_LinearProgressionExercise_EquipmentStackId",
                table: "LinearProgressionExercise");

            migrationBuilder.DropColumn(
                name: "EquipmentStackId",
                table: "LinearProgressionExercise");

            migrationBuilder.DropColumn(
                name: "EquipmentStackKey",
                table: "LinearProgressionExercise");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Exercises",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

        }
    }
}
