using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class comunicadonovoscampos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DestinatarioDexCoordenador",
                table: "Comunicado",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DestinatarioUnidadeDiretor",
                table: "Comunicado",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DestinatarioUnidadeSupervisor",
                table: "Comunicado",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestinatarioDexCoordenador",
                table: "Comunicado");

            migrationBuilder.DropColumn(
                name: "DestinatarioUnidadeDiretor",
                table: "Comunicado");

            migrationBuilder.DropColumn(
                name: "DestinatarioUnidadeSupervisor",
                table: "Comunicado");
        }
    }
}
