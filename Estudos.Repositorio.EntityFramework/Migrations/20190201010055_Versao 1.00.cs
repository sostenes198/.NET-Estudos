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
                name: "Cardapio_Categoria",
                columns: table => new
                {
                    CardCat_Codigo = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CardCat_Descricao = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cardapio_Categoria", x => x.CardCat_Codigo);
                });

            migrationBuilder.CreateTable(
                name: "Pedido_Completo",
                columns: table => new
                {
                    PedComp_Codigo = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PedComp_CodigoMesa = table.Column<int>(nullable: false),
                    NomeMesa = table.Column<string>(maxLength: 10, nullable: false),
                    PedComp_Data = table.Column<DateTime>(nullable: false),
                    PedComp_ValorTotal = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PedComp_TipoPagamento = table.Column<int>(nullable: false),
                    PedComp_ValorTroco = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedido_Completo", x => x.PedComp_Codigo);
                });

            migrationBuilder.CreateTable(
                name: "Cardapio",
                columns: table => new
                {
                    Card_Codigo = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Card_Titulo = table.Column<string>(maxLength: 100, nullable: false),
                    Card_Descricao = table.Column<string>(maxLength: 100, nullable: false),
                    CardCat_Codigo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cardapio", x => x.Card_Codigo);
                    table.ForeignKey(
                        name: "FK_Cardapio_Cardapio_Categoria_CardCat_Codigo",
                        column: x => x.CardCat_Codigo,
                        principalTable: "Cardapio_Categoria",
                        principalColumn: "CardCat_Codigo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedido",
                columns: table => new
                {
                    Ped_Codigo = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ped_Observacao = table.Column<string>(maxLength: 100, nullable: false),
                    Quantidade = table.Column<int>(nullable: false),
                    Ped_ValorTotalPedido = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Card_Codigo = table.Column<int>(nullable: false),
                    PedComp_Codigo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedido", x => x.Ped_Codigo);
                    table.ForeignKey(
                        name: "FK_Pedido_Cardapio_Card_Codigo",
                        column: x => x.Card_Codigo,
                        principalTable: "Cardapio",
                        principalColumn: "Card_Codigo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pedido_Pedido_Completo_PedComp_Codigo",
                        column: x => x.PedComp_Codigo,
                        principalTable: "Pedido_Completo",
                        principalColumn: "PedComp_Codigo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cardapio_CardCat_Codigo",
                table: "Cardapio",
                column: "CardCat_Codigo");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_Card_Codigo",
                table: "Pedido",
                column: "Card_Codigo");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_PedComp_Codigo",
                table: "Pedido",
                column: "PedComp_Codigo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pedido");

            migrationBuilder.DropTable(
                name: "Cardapio");

            migrationBuilder.DropTable(
                name: "Pedido_Completo");

            migrationBuilder.DropTable(
                name: "Cardapio_Categoria");
        }
    }
}
