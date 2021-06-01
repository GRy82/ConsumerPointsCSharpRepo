using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsumerPoints.Migrations
{
    public partial class Init : Migration
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
                name: "Transactions",
                columns: table => new
                {
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Payer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Timestamp);
                });

            migrationBuilder.InsertData(
                table: "PayerPoints",
                columns: new[] { "Payer", "Points" },
                values: new object[] { "cvs pharmacy", 600 });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Timestamp", "Payer", "Points" },
                values: new object[] { new DateTime(2021, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "CVS Pharmacy", 400 });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Timestamp", "Payer", "Points" },
                values: new object[] { new DateTime(2021, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "CVS Pharmacy", 200 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayerPoints");

            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
