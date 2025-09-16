using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace xml_reader.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotasFiscais",
                columns: table => new
                {
                    IdNotaFiscal = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PrestadorCNPJ = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    TomadorCNPJ = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    ServicoDescricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ServicoValor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DataEmissao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NomeArquivo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotasFiscais", x => x.IdNotaFiscal);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotasFiscais");
        }
    }
}
