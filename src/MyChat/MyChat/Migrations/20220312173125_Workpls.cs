using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyChat.Migrations
{
    public partial class Workpls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomUsersContext_RoomContext_RoomId",
                table: "RoomUsersContext");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomUsersContext",
                table: "RoomUsersContext");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomContext",
                table: "RoomContext");

            migrationBuilder.RenameTable(
                name: "RoomUsersContext",
                newName: "RoomUsers");

            migrationBuilder.RenameTable(
                name: "RoomContext",
                newName: "Rooms");

            migrationBuilder.RenameIndex(
                name: "IX_RoomUsersContext_RoomId",
                table: "RoomUsers",
                newName: "IX_RoomUsers_RoomId");

            migrationBuilder.RenameColumn(
                name: "IdentityUser",
                table: "Rooms",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "RoomUsers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUser",
                table: "RoomUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "RoomUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "AdminId",
                table: "Rooms",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomUsers",
                table: "RoomUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomUsers_Rooms_RoomId",
                table: "RoomUsers",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomUsers_Rooms_RoomId",
                table: "RoomUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomUsers",
                table: "RoomUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RoomUsers");

            migrationBuilder.RenameTable(
                name: "RoomUsers",
                newName: "RoomUsersContext");

            migrationBuilder.RenameTable(
                name: "Rooms",
                newName: "RoomContext");

            migrationBuilder.RenameIndex(
                name: "IX_RoomUsers_RoomId",
                table: "RoomUsersContext",
                newName: "IX_RoomUsersContext_RoomId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "RoomContext",
                newName: "IdentityUser");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "RoomUsersContext",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUser",
                table: "RoomUsersContext",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AdminId",
                table: "RoomContext",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomUsersContext",
                table: "RoomUsersContext",
                column: "IdentityUser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomContext",
                table: "RoomContext",
                column: "IdentityUser");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomUsersContext_RoomContext_RoomId",
                table: "RoomUsersContext",
                column: "RoomId",
                principalTable: "RoomContext",
                principalColumn: "IdentityUser",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
