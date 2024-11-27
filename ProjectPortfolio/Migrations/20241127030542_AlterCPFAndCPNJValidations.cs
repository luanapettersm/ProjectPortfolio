using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectPortfolio.Migrations
{
    /// <inheritdoc />
    public partial class AlterCPFAndCPNJValidations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CNPJNumber",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "Clients",
                newName: "CPF");

            migrationBuilder.RenameColumn(
                name: "CPFNumber",
                table: "Clients",
                newName: "CNPJ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CPF",
                table: "Clients",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "CNPJ",
                table: "Clients",
                newName: "CPFNumber");

            migrationBuilder.AddColumn<string>(
                name: "CNPJNumber",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
