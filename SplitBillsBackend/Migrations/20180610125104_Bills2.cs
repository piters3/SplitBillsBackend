using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SplitBillsBackend.Migrations
{
    public partial class Bills2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Bills_BillId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Bills_AspNetUsers_UserId",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Bills_UserId",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BillId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "BillId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "UserBill",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    BillId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBill", x => new { x.UserId, x.BillId });
                    table.ForeignKey(
                        name: "FK_UserBill_Bills_BillId",
                        column: x => x.BillId,
                        principalTable: "Bills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBill_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBill_BillId",
                table: "UserBill",
                column: "BillId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBill");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Bills",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BillId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bills_UserId",
                table: "Bills",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BillId",
                table: "AspNetUsers",
                column: "BillId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Bills_BillId",
                table: "AspNetUsers",
                column: "BillId",
                principalTable: "Bills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_AspNetUsers_UserId",
                table: "Bills",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
