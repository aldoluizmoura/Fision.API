using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FIsionAPI.Data.Migrations
{
    public partial class reparorelacionamentos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContratoFinanceiro_Entidades_EntidadeId",
                table: "ContratoFinanceiro");

            migrationBuilder.DropIndex(
                name: "IX_EnderecoPessoa_PessoaId",
                table: "EnderecoPessoa");

            migrationBuilder.DropIndex(
                name: "IX_ContratoFinanceiro_EntidadeId",
                table: "ContratoFinanceiro");

            migrationBuilder.DropColumn(
                name: "EntidadeId",
                table: "ContratoFinanceiro");

            migrationBuilder.RenameColumn(
                name: "varchar(100)",
                table: "EnderecoPessoa",
                newName: "Complemento");

            migrationBuilder.AddColumn<Guid>(
                name: "ContratoId",
                table: "Entidades",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "PessoaId",
                table: "EnderecoPessoa",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCadastro",
                table: "ContratoFinanceiro",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Entidades_ContratoId",
                table: "Entidades",
                column: "ContratoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoPessoa_PessoaId",
                table: "EnderecoPessoa",
                column: "PessoaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Entidades_ContratoFinanceiro_ContratoId",
                table: "Entidades",
                column: "ContratoId",
                principalTable: "ContratoFinanceiro",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entidades_ContratoFinanceiro_ContratoId",
                table: "Entidades");

            migrationBuilder.DropIndex(
                name: "IX_Entidades_ContratoId",
                table: "Entidades");

            migrationBuilder.DropIndex(
                name: "IX_EnderecoPessoa_PessoaId",
                table: "EnderecoPessoa");

            migrationBuilder.DropColumn(
                name: "ContratoId",
                table: "Entidades");

            migrationBuilder.DropColumn(
                name: "DataCadastro",
                table: "ContratoFinanceiro");

            migrationBuilder.RenameColumn(
                name: "Complemento",
                table: "EnderecoPessoa",
                newName: "varchar(100)");

            migrationBuilder.AlterColumn<Guid>(
                name: "PessoaId",
                table: "EnderecoPessoa",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "EntidadeId",
                table: "ContratoFinanceiro",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoPessoa_PessoaId",
                table: "EnderecoPessoa",
                column: "PessoaId",
                unique: true,
                filter: "[PessoaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ContratoFinanceiro_EntidadeId",
                table: "ContratoFinanceiro",
                column: "EntidadeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ContratoFinanceiro_Entidades_EntidadeId",
                table: "ContratoFinanceiro",
                column: "EntidadeId",
                principalTable: "Entidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
