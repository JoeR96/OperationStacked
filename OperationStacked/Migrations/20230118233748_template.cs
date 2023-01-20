using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OperationStacked.Migrations
{
    public partial class template : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EquipmentType",
                table: "Exercises",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.AddColumn<decimal>(
                name: "Intensity",
                table: "Exercises",
                type: "decimal(65,30)",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmrapRepResult",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "AmrapRepTarget",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "AuxillaryLift",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "Block",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "Intensity",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "RepsPerSet",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "Sets",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "TrainingMax",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "Week",
                table: "Exercises");

            migrationBuilder.AlterColumn<string>(
                name: "EquipmentType",
                table: "Exercises",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
