using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectPortfolio.Migrations
{
    /// <inheritdoc />
    public partial class CreateBrazilianStates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "States",
            columns: ["Id", "Name", "UF"],
            values: new object[,]
            {
                { Guid.NewGuid(), "Acre", "AC" },
                { Guid.NewGuid(), "Alagoas", "AL" },
                { Guid.NewGuid(), "Amapá", "AP" },
                { Guid.NewGuid(), "Amazonas", "AM" },
                { Guid.NewGuid(), "Bahia", "BA" },
                { Guid.NewGuid(), "Ceará", "CE" },
                { Guid.NewGuid(), "Distrito Federal", "DF" },
                { Guid.NewGuid(), "Espírito Santo", "ES" },
                { Guid.NewGuid(), "Goiás", "GO" },
                { Guid.NewGuid(), "Maranhão", "MA" },
                { Guid.NewGuid(), "Mato Grosso", "MT" },
                { Guid.NewGuid(), "Mato Grosso do Sul", "MS" },
                { Guid.NewGuid(), "Minas Gerais", "MG" },
                { Guid.NewGuid(), "Pará", "PA" },
                { Guid.NewGuid(), "Paraíba", "PB" },
                { Guid.NewGuid(), "Paraná", "PR" },
                { Guid.NewGuid(), "Pernambuco", "PE" },
                { Guid.NewGuid(), "Piauí", "PI" },
                { Guid.NewGuid(), "Rio de Janeiro", "RJ" },
                { Guid.NewGuid(), "Rio Grande do Norte", "RN" },
                { Guid.NewGuid(), "Rio Grande do Sul", "RS" },
                { Guid.NewGuid(), "Rondônia", "RO" },
                { Guid.NewGuid(), "Roraima", "RR" },
                { Guid.NewGuid(), "Santa Catarina", "SC" },
                { Guid.NewGuid(), "São Paulo", "SP" },
                { Guid.NewGuid(), "Sergipe", "SE" },
                { Guid.NewGuid(), "Tocantins", "TO" }
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM States");
        }
    }
}
