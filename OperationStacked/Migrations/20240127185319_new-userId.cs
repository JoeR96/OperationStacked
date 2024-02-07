using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OperationStacked.Migrations
{
    public partial class newuserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Users_UserAccountUserId",
                table: "UserRole");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserAccountUserId",
                table: "UserRole",
                newName: "UserAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRole_UserAccountUserId",
                table: "UserRole",
                newName: "IX_UserRole_UserAccountId");

            migrationBuilder.RenameColumn(
                name: "EquipmentStackKey",
                table: "EquipmentStacks",
                newName: "Name");

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

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Users_UserAccountId",
                table: "UserRole",
                column: "UserAccountId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Users_UserAccountId",
                table: "UserRole");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "UserAccountId",
                table: "UserRole",
                newName: "UserAccountUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRole_UserAccountId",
                table: "UserRole",
                newName: "IX_UserRole_UserAccountUserId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "EquipmentStacks",
                newName: "EquipmentStackKey");

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

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Users_UserAccountUserId",
                table: "UserRole",
                column: "UserAccountUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
