using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FIsionAPI.Data.Migrations
{
    public partial class AjusteMovimentos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovimentoFinanceiro");

            migrationBuilder.CreateTable(
                name: "MovimentoFinanceiroAvulso",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Classe = table.Column<int>(type: "int", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Situacao = table.Column<int>(type: "int", nullable: false),
                    DtPagamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompetenciaMensalidade = table.Column<string>(type: "varchar(100)", nullable: true),
                    Descricao = table.Column<string>(type: "varchar(100)", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    CaixaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimentoFinanceiroAvulso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimentoFinanceiroAvulso_Caixa_CaixaId",
                        column: x => x.CaixaId,
                        principalTable: "Caixa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MovimentoFinanceiroEntidade",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Classe = table.Column<int>(type: "int", nullable: false),
                    ValorReceber = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorPago = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorMensal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Desconto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Situacao = table.Column<int>(type: "int", nullable: false),
                    DtPagamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtVencimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompetenciaMensalidade = table.Column<string>(type: "varchar(100)", nullable: true),
                    CompetenciaPagamento = table.Column<string>(type: "varchar(100)", nullable: true),
                    QuantidadeAlunos = table.Column<int>(type: "int", nullable: true),
                    Observacao = table.Column<string>(type: "varchar(100)", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContratoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    CaixaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimentoFinanceiroEntidade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimentoFinanceiroEntidade_Caixa_CaixaId",
                        column: x => x.CaixaId,
                        principalTable: "Caixa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimentoFinanceiroEntidade_ContratoFinanceiro_ContratoId",
                        column: x => x.ContratoId,
                        principalTable: "ContratoFinanceiro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovimentoFinanceiroAvulso_CaixaId",
                table: "MovimentoFinanceiroAvulso",
                column: "CaixaId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentoFinanceiroEntidade_CaixaId",
                table: "MovimentoFinanceiroEntidade",
                column: "CaixaId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentoFinanceiroEntidade_ContratoId",
                table: "MovimentoFinanceiroEntidade",
                column: "ContratoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovimentoFinanceiroAvulso");

            migrationBuilder.DropTable(
                name: "MovimentoFinanceiroEntidade");

            migrationBuilder.CreateTable(
                name: "MovimentoFinanceiro",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaixaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Classe = table.Column<int>(type: "int", nullable: false),
                    CompetenciaMensalidade = table.Column<string>(type: "varchar(100)", nullable: true),
                    CompetenciaPagamento = table.Column<string>(type: "varchar(100)", nullable: true),
                    ContratoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Desconto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DtPagamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtVencimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observacao = table.Column<string>(type: "varchar(100)", nullable: true),
                    QuantidadeAlunos = table.Column<int>(type: "int", nullable: true),
                    Situacao = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    ValorMensal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorPago = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorReceber = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimentoFinanceiro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimentoFinanceiro_Caixa_CaixaId",
                        column: x => x.CaixaId,
                        principalTable: "Caixa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimentoFinanceiro_ContratoFinanceiro_ContratoId",
                        column: x => x.ContratoId,
                        principalTable: "ContratoFinanceiro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovimentoFinanceiro_CaixaId",
                table: "MovimentoFinanceiro",
                column: "CaixaId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentoFinanceiro_ContratoId",
                table: "MovimentoFinanceiro",
                column: "ContratoId");
        }
    }
}
