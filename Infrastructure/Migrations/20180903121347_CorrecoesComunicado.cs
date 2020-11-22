using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class CorrecoesComunicado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Areas",
                table: "Comunicado");

            migrationBuilder.DropColumn(
                name: "Cargos",
                table: "Comunicado");

            migrationBuilder.DropColumn(
                name: "Funcionarios",
                table: "Comunicado");

            migrationBuilder.AddColumn<bool>(
                name: "DestinatarioDex",
                table: "Comunicado",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DestinatarioUnidade",
                table: "Comunicado",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestinatarioDex",
                table: "Comunicado");

            migrationBuilder.DropColumn(
                name: "DestinatarioUnidade",
                table: "Comunicado");

            migrationBuilder.AddColumn<string>(
                name: "Areas",
                table: "Comunicado",
                type: "varchar(MAX)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cargos",
                table: "Comunicado",
                type: "varchar(MAX)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Funcionarios",
                table: "Comunicado",
                type: "varchar(MAX)",
                nullable: false,
                defaultValue: "");
        }
    }
}
