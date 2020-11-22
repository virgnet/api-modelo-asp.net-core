using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class comunicadonovoscampos2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Aprovado",
                table: "ComunicadoVersao",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DestinatarioUnidadeCaf",
                table: "Comunicado",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DestinatarioUnidadeCoordenadorSenat",
                table: "Comunicado",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DestinatarioUnidadeCoordenadorSest",
                table: "Comunicado",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aprovado",
                table: "ComunicadoVersao");

            migrationBuilder.DropColumn(
                name: "DestinatarioUnidadeCaf",
                table: "Comunicado");

            migrationBuilder.DropColumn(
                name: "DestinatarioUnidadeCoordenadorSenat",
                table: "Comunicado");

            migrationBuilder.DropColumn(
                name: "DestinatarioUnidadeCoordenadorSest",
                table: "Comunicado");
        }
    }
}
