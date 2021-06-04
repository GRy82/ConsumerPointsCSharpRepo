using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsumerPoints.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PayerPoints",
                columns: table => new
                {
                    Payer = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayerPoints", x => x.Payer);
                });

            migrationBuilder.CreateTable(
                name: "SpendingMarkers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastSpentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastWasPartiallySpent = table.Column<bool>(type: "bit", nullable: false),
                    Remainder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpendingMarkers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Payer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Points = table.Column<int>(type: "int", nullable: false),
                    PointsSpent = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Timestamp);
                });

            migrationBuilder.InsertData(
                table: "SpendingMarkers",
                columns: new[] { "Id", "LastSpentDate", "LastWasPartiallySpent", "Remainder" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayerPoints");

            migrationBuilder.DropTable(
                name: "SpendingMarkers");

            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
