using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Estudos.Repositorio.EntityFrameworkCore.Migrations
{
    public partial class Versao100 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Cardapio_Categoria",
                table => new
                {
                    CardCat_Codigo = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CardCat_Descricao = table.Column<string>(maxLength: 100)
                },
                constraints: table => table.PrimaryKey("PK_Cardapio_Categoria", x => x.CardCat_Codigo));

            migrationBuilder.CreateTable(
                "Pedido_Completo",
                table => new
                {
                    PedComp_Codigo = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PedComp_CodigoMesa = table.Column<int>(),
                    NomeMesa = table.Column<string>(maxLength: 10),
                    PedComp_Data = table.Column<DateTime>(),
                    PedComp_ValorTotal = table.Column<decimal>("decimal(18, 2)"),
                    PedComp_TipoPagamento = table.Column<int>(),
                    PedComp_ValorTroco = table.Column<decimal>(nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_Pedido_Completo", x => x.PedComp_Codigo));

            migrationBuilder.CreateTable(
                "Cardapio",
                table => new
                {
                    Card_Codigo = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Card_Titulo = table.Column<string>(maxLength: 100),
                    Card_Descricao = table.Column<string>(maxLength: 100),
                    CardCat_Codigo = table.Column<int>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cardapio", x => x.Card_Codigo);
                    table.ForeignKey(
                        "FK_Cardapio_Cardapio_Categoria_CardCat_Codigo",
                        x => x.CardCat_Codigo,
                        "Cardapio_Categoria",
                        "CardCat_Codigo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Pedido",
                table => new
                {
                    Ped_Codigo = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ped_Observacao = table.Column<string>(maxLength: 100),
                    Quantidade = table.Column<int>(),
                    Ped_ValorTotalPedido = table.Column<decimal>("decimal(18, 2)"),
                    Card_Codigo = table.Column<int>(),
                    PedComp_Codigo = table.Column<int>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedido", x => x.Ped_Codigo);
                    table.ForeignKey(
                        "FK_Pedido_Cardapio_Card_Codigo",
                        x => x.Card_Codigo,
                        "Cardapio",
                        "Card_Codigo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Pedido_Pedido_Completo_PedComp_Codigo",
                        x => x.PedComp_Codigo,
                        "Pedido_Completo",
                        "PedComp_Codigo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Cardapio_CardCat_Codigo",
                "Cardapio",
                "CardCat_Codigo");

            migrationBuilder.CreateIndex(
                "IX_Pedido_Card_Codigo",
                "Pedido",
                "Card_Codigo");

            migrationBuilder.CreateIndex(
                "IX_Pedido_PedComp_Codigo",
                "Pedido",
                "PedComp_Codigo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Pedido");

            migrationBuilder.DropTable(
                "Cardapio");

            migrationBuilder.DropTable(
                "Pedido_Completo");

            migrationBuilder.DropTable(
                "Cardapio_Categoria");
        }
    }
}