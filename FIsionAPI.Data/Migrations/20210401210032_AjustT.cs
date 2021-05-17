using Microsoft.EntityFrameworkCore.Migrations;

namespace FIsionAPI.Data.Migrations
{
    public partial class AjustT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Classe",
                table: "MovimentoFinanceiro",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Classe",
                table: "MovimentoFinanceiro");
        }
    }
}
