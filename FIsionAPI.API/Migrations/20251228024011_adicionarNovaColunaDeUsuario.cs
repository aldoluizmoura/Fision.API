using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIsionAPI.API.Migrations
{
    /// <inheritdoc />
    public partial class adicionarNovaColunaDeUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Documento",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Documento",
                table: "Usuarios");
        }
    }
}
