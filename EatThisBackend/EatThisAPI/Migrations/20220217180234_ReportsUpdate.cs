using Microsoft.EntityFrameworkCore.Migrations;

namespace EatThisAPI.Migrations
{
    public partial class ReportsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_ReportStatuses_ReportStatusId1",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ReportStatusId1",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReportStatusId1",
                table: "Reports");

            migrationBuilder.AlterColumn<int>(
                name: "ReportStatusId",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportStatusId",
                table: "Reports",
                column: "ReportStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_ReportStatuses_ReportStatusId",
                table: "Reports",
                column: "ReportStatusId",
                principalTable: "ReportStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_ReportStatuses_ReportStatusId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ReportStatusId",
                table: "Reports");

            migrationBuilder.AlterColumn<string>(
                name: "ReportStatusId",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ReportStatusId1",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportStatusId1",
                table: "Reports",
                column: "ReportStatusId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_ReportStatuses_ReportStatusId1",
                table: "Reports",
                column: "ReportStatusId1",
                principalTable: "ReportStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
