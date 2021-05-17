using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FIsionAPI.Data.Migrations
{
    public partial class reparorelacionamentos2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entidades_ContratoFinanceiro_ContratoId",
                table: "Entidades");

            migrationBuilder.DropIndex(
                name: "IX_Entidades_ContratoId",
                table: "Entidades");

            migrationBuilder.DropColumn(
                name: "ContratoId",
                table: "Entidades");

            migrationBuilder.AddColumn<Guid>(
                name: "EntidadeId",
                table: "ContratoFinanceiro",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContratoFinanceiro_Entidades_EntidadeId",
                table: "ContratoFinanceiro");

            migrationBuilder.DropIndex(
                name: "IX_ContratoFinanceiro_EntidadeId",
                table: "ContratoFinanceiro");

            migrationBuilder.DropColumn(
                name: "EntidadeId",
                table: "ContratoFinanceiro");

            migrationBuilder.AddColumn<Guid>(
                name: "ContratoId",
                table: "Entidades",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Entidades_ContratoId",
                table: "Entidades",
                column: "ContratoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Entidades_ContratoFinanceiro_ContratoId",
                table: "Entidades",
                column: "ContratoId",
                principalTable: "ContratoFinanceiro",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
