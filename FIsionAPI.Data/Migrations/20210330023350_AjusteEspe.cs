using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FIsionAPI.Data.Migrations
{
    public partial class AjusteEspe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Especialidade",
                table: "Entidades");

            migrationBuilder.AddColumn<Guid>(
                name: "EspecialidadeId",
                table: "Entidades",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Especialidades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(100)", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especialidades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntidadeEspecialidades",
                columns: table => new
                {
                    EntidadesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EspecialidadesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntidadeEspecialidades", x => new { x.EntidadesId, x.EspecialidadesId });
                    table.ForeignKey(
                        name: "FK_EntidadeEspecialidades_Entidades_EntidadesId",
                        column: x => x.EntidadesId,
                        principalTable: "Entidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntidadeEspecialidades_Especialidades_EspecialidadesId",
                        column: x => x.EspecialidadesId,
                        principalTable: "Especialidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntidadeEspecialidades_EspecialidadesId",
                table: "EntidadeEspecialidades",
                column: "EspecialidadesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntidadeEspecialidades");

            migrationBuilder.DropTable(
                name: "Especialidades");

            migrationBuilder.DropColumn(
                name: "EspecialidadeId",
                table: "Entidades");

            migrationBuilder.AddColumn<int>(
                name: "Especialidade",
                table: "Entidades",
                type: "int",
                nullable: true);
        }
    }
}
