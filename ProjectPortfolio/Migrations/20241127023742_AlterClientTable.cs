using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectPortfolio.Migrations
{
    /// <inheritdoc />
    public partial class AlterClientTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "Issues",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttendantName",
                table: "Issues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "Issues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Solution",
                table: "Issues",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendantName",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "Solution",
                table: "Issues");

            migrationBuilder.AlterColumn<string>(
                name: "Priority",
                table: "Issues",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
