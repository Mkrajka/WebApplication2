using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class ChaningTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Member_AspNetUsers_LoginId",
                table: "Member");

            migrationBuilder.RenameColumn(
                name: "LoginId",
                table: "Member",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Member_LoginId",
                table: "Member",
                newName: "IX_Member_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Member_AspNetUsers_ApplicationUserId",
                table: "Member",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Member_AspNetUsers_ApplicationUserId",
                table: "Member");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Member",
                newName: "LoginId");

            migrationBuilder.RenameIndex(
                name: "IX_Member_ApplicationUserId",
                table: "Member",
                newName: "IX_Member_LoginId");

            migrationBuilder.AddForeignKey(
                name: "FK_Member_AspNetUsers_LoginId",
                table: "Member",
                column: "LoginId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
