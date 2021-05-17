using Microsoft.EntityFrameworkCore.Migrations;

namespace FIsionAPI.Data.Migrations
{
    public partial class AjusteMF : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ValorEmAberto",
                table: "MovimentoFinanceiro",
                newName: "ValorTotal");

            migrationBuilder.RenameColumn(
                name: "Competencia",
                table: "MovimentoFinanceiro",
                newName: "Observacao");

            migrationBuilder.AddColumn<string>(
                name: "CompetenciaMensalidade",
                table: "MovimentoFinanceiro",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompetenciaPagamento",
                table: "MovimentoFinanceiro",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Desconto",
                table: "MovimentoFinanceiro",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantidadeAlunos",
                table: "MovimentoFinanceiro",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorMensal",
                table: "MovimentoFinanceiro",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorReceber",
                table: "MovimentoFinanceiro",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorMensal",
                table: "ContratoFinanceiro",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorUnitario",
                table: "ContratoFinanceiro",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompetenciaMensalidade",
                table: "MovimentoFinanceiro");

            migrationBuilder.DropColumn(
                name: "CompetenciaPagamento",
                table: "MovimentoFinanceiro");

            migrationBuilder.DropColumn(
                name: "Desconto",
                table: "MovimentoFinanceiro");

            migrationBuilder.DropColumn(
                name: "QuantidadeAlunos",
                table: "MovimentoFinanceiro");

            migrationBuilder.DropColumn(
                name: "ValorMensal",
                table: "MovimentoFinanceiro");

            migrationBuilder.DropColumn(
                name: "ValorReceber",
                table: "MovimentoFinanceiro");

            migrationBuilder.DropColumn(
                name: "ValorUnitario",
                table: "ContratoFinanceiro");

            migrationBuilder.RenameColumn(
                name: "ValorTotal",
                table: "MovimentoFinanceiro",
                newName: "ValorEmAberto");

            migrationBuilder.RenameColumn(
                name: "Observacao",
                table: "MovimentoFinanceiro",
                newName: "Competencia");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorMensal",
                table: "ContratoFinanceiro",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }
    }
}
