using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class AddingNewConnections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Member_AspNetUsers_ApplicationUserId",
                table: "Member");

            migrationBuilder.DropIndex(
                name: "IX_Member_ApplicationUserId",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Member");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Member",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Member",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Member_UserId",
                table: "Member",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Member_AspNetUsers_UserId",
                table: "Member",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Member_AspNetUsers_UserId",
                table: "Member");

            migrationBuilder.DropIndex(
                name: "IX_Member_UserId",
                table: "Member");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Member",
                newName: "UserID");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Member",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Member",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Member_ApplicationUserId",
                table: "Member",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Member_AspNetUsers_ApplicationUserId",
                table: "Member",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
