using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OperationStacked.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "CognitoUserId");

            migrationBuilder.RenameColumn(
                name: "AuxillaryLift",
                table: "A2SHypertrophyExercise",
                newName: "PrimaryLift");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Exercises",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "Exercises",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "Exercises");

            migrationBuilder.RenameColumn(
                name: "CognitoUserId",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "PrimaryLift",
                table: "A2SHypertrophyExercise",
                newName: "AuxillaryLift");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Exercises",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
