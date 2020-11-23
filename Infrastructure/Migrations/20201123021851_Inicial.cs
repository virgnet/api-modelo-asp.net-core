using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Acesso",
                columns: table => new
                {
                    IdAcesso = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Login = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Senha = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    TempId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acesso", x => x.IdAcesso);
                    table.UniqueConstraint("AK_Acesso_TempId", x => x.TempId);
                });

            migrationBuilder.CreateTable(
                name: "Publicacao",
                columns: table => new
                {
                    IdPublicacao = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Identificador = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Titulo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Conteudo = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataPublicacao = table.Column<DateTime>(type: "datetime", nullable: false),
                    TempId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publicacao", x => x.IdPublicacao);
                    table.UniqueConstraint("AK_Publicacao_TempId", x => x.TempId);
                });

            migrationBuilder.CreateTable(
                name: "Tema",
                columns: table => new
                {
                    IdTema = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    TempId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tema", x => x.IdTema);
                    table.UniqueConstraint("AK_Tema_TempId1", x => x.TempId1);
                });

            migrationBuilder.CreateTable(
                name: "PublicacaoTema",
                columns: table => new
                {
                    IdPublicacao = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdTema = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TempId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicacaoTema", x => new { x.IdPublicacao, x.IdTema });
                    table.UniqueConstraint("AK_PublicacaoTema_TempId", x => x.TempId);
                    table.ForeignKey(
                        name: "FK_PublicacaoTema_Publicacao_IdPublicacao",
                        column: x => x.IdPublicacao,
                        principalTable: "Publicacao",
                        principalColumn: "IdPublicacao",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PublicacaoTema_Tema_IdTema",
                        column: x => x.IdTema,
                        principalTable: "Tema",
                        principalColumn: "IdTema",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PublicacaoTema_IdTema",
                table: "PublicacaoTema",
                column: "IdTema");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Acesso");

            migrationBuilder.DropTable(
                name: "PublicacaoTema");

            migrationBuilder.DropTable(
                name: "Publicacao");

            migrationBuilder.DropTable(
                name: "Tema");
        }
    }
}
