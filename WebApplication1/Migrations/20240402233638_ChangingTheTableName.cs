using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class ChangingTheTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WhatsappMessages_AspNetUsers_UserId",
                table: "WhatsappMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WhatsappMessages",
                table: "WhatsappMessages");

            migrationBuilder.RenameTable(
                name: "WhatsappMessages",
                newName: "Emails");

            migrationBuilder.RenameIndex(
                name: "IX_WhatsappMessages_UserId",
                table: "Emails",
                newName: "IX_Emails_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Emails",
                table: "Emails",
                column: "EmailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Emails_AspNetUsers_UserId",
                table: "Emails",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emails_AspNetUsers_UserId",
                table: "Emails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Emails",
                table: "Emails");

            migrationBuilder.RenameTable(
                name: "Emails",
                newName: "WhatsappMessages");

            migrationBuilder.RenameIndex(
                name: "IX_Emails_UserId",
                table: "WhatsappMessages",
                newName: "IX_WhatsappMessages_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WhatsappMessages",
                table: "WhatsappMessages",
                column: "EmailId");

            migrationBuilder.AddForeignKey(
                name: "FK_WhatsappMessages_AspNetUsers_UserId",
                table: "WhatsappMessages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
