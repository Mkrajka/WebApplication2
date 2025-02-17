using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class AddingCalorieEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalorieEntry",
                columns: table => new
                {
                    CalorieID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatePaid = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BreakfastCalories = table.Column<int>(type: "int", nullable: false),
                    LunchCalories = table.Column<int>(type: "int", nullable: false),
                    DinnerCalories = table.Column<int>(type: "int", nullable: false),
                    SnackCalories = table.Column<int>(type: "int", nullable: false),
                    MemberID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalorieEntry", x => x.CalorieID);
                    table.ForeignKey(
                        name: "FK_CalorieEntry_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalorieEntry_MemberID",
                table: "CalorieEntry",
                column: "MemberID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalorieEntry");
        }
    }
}
