using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class Inicio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comunicado",
                columns: table => new
                {
                    IdComunicado = table.Column<Guid>(nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataPublicacao = table.Column<DateTime>(type: "datetime", nullable: false),
                    Areas = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    Cargos = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    Funcionarios = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    Ativo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comunicado", x => x.IdComunicado);
                });

            migrationBuilder.CreateTable(
                name: "Tema",
                columns: table => new
                {
                    IdTema = table.Column<Guid>(nullable: false),
                    Titulo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Tags = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    Ativo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tema", x => x.IdTema);
                });

            migrationBuilder.CreateTable(
                name: "TipoIndicador",
                columns: table => new
                {
                    IdTipoIndicador = table.Column<Guid>(nullable: false),
                    Sigla = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Titulo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoIndicador", x => x.IdTipoIndicador);
                });

            migrationBuilder.CreateTable(
                name: "Calendario",
                columns: table => new
                {
                    IdCalendario = table.Column<Guid>(nullable: false),
                    IdTema = table.Column<Guid>(nullable: true),
                    Identificador = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Local = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Titulo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: false),
                    Ativo = table.Column<bool>(nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendario", x => x.IdCalendario);
                    table.ForeignKey(
                        name: "FK_Calendario_Tema_IdTema",
                        column: x => x.IdTema,
                        principalTable: "Tema",
                        principalColumn: "IdTema",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TemaRelacionamento",
                columns: table => new
                {
                    IdTemaRelacionamento = table.Column<Guid>(nullable: false),
                    IdTema = table.Column<Guid>(nullable: false),
                    IdArea = table.Column<Guid>(nullable: true),
                    IdProjeto = table.Column<Guid>(nullable: true),
                    IdSistema = table.Column<Guid>(nullable: true),
                    IdDocumento = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemaRelacionamento", x => x.IdTemaRelacionamento);
                    table.ForeignKey(
                        name: "FK_TemaRelacionamento_Tema_IdTema",
                        column: x => x.IdTema,
                        principalTable: "Tema",
                        principalColumn: "IdTema",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Indicador",
                columns: table => new
                {
                    IdIndicador = table.Column<Guid>(nullable: false),
                    IdTipoIndicador = table.Column<Guid>(nullable: false),
                    Titulo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indicador", x => x.IdIndicador);
                    table.ForeignKey(
                        name: "FK_Indicador_TipoIndicador_IdTipoIndicador",
                        column: x => x.IdTipoIndicador,
                        principalTable: "TipoIndicador",
                        principalColumn: "IdTipoIndicador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComunicadoVersao",
                columns: table => new
                {
                    IdComunicadoVersao = table.Column<Guid>(nullable: false),
                    IdComunicado = table.Column<Guid>(nullable: false),
                    IdTemplate = table.Column<Guid>(nullable: false),
                    IdTema = table.Column<Guid>(nullable: true),
                    Versao = table.Column<int>(nullable: false),
                    Assunto = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Conteudo = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    Ativo = table.Column<bool>(nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComunicadoVersao", x => x.IdComunicadoVersao);
                    table.ForeignKey(
                        name: "FK_ComunicadoVersao_Comunicado_IdComunicado",
                        column: x => x.IdComunicado,
                        principalTable: "Comunicado",
                        principalColumn: "IdComunicado",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComunicadoVersao_Tema_IdTema",
                        column: x => x.IdTema,
                        principalTable: "Tema",
                        principalColumn: "IdTema",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComunicadoVersao_Indicador_IdTemplate",
                        column: x => x.IdTemplate,
                        principalTable: "Indicador",
                        principalColumn: "IdIndicador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContatoDiex",
                columns: table => new
                {
                    IdContatoDiex = table.Column<Guid>(nullable: false),
                    IdAssunto = table.Column<Guid>(nullable: false),
                    LoginRemetente = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Mensagem = table.Column<string>(type: "varchar(MAX)", maxLength: 100, nullable: false),
                    DataMensagem = table.Column<DateTime>(type: "datetime", nullable: false),
                    LoginResposta = table.Column<string>(type: "varchar(100)", nullable: true),
                    Resposta = table.Column<string>(type: "varchar(MAX)", maxLength: 500, nullable: true),
                    DataResposta = table.Column<DateTime>(type: "datetime", nullable: true),
                    Ativo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContatoDiex", x => x.IdContatoDiex);
                    table.ForeignKey(
                        name: "FK_ContatoDiex_Indicador_IdAssunto",
                        column: x => x.IdAssunto,
                        principalTable: "Indicador",
                        principalColumn: "IdIndicador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Publicacao",
                columns: table => new
                {
                    IdPublicacao = table.Column<Guid>(nullable: false),
                    IdTema = table.Column<Guid>(nullable: true),
                    IdTipoDeConteudo = table.Column<Guid>(nullable: false),
                    Identificador = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Titulo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Chamada = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: true),
                    Conteudo = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    Tags = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    Ativo = table.Column<bool>(nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataPublicacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Binario = table.Column<byte[]>(nullable: true),
                    ImagemCapa = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publicacao", x => x.IdPublicacao);
                    table.ForeignKey(
                        name: "FK_Publicacao_Tema_IdTema",
                        column: x => x.IdTema,
                        principalTable: "Tema",
                        principalColumn: "IdTema",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Publicacao_Indicador_IdTipoDeConteudo",
                        column: x => x.IdTipoDeConteudo,
                        principalTable: "Indicador",
                        principalColumn: "IdIndicador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComunicadoVersaoLog",
                columns: table => new
                {
                    IdComunicadoVersaoLog = table.Column<Guid>(nullable: false),
                    IdComunicadoVersao = table.Column<Guid>(nullable: false),
                    Login = table.Column<string>(type: "varchar(100)", maxLength: 50, nullable: false),
                    DataLeitura = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComunicadoVersaoLog", x => x.IdComunicadoVersaoLog);
                    table.ForeignKey(
                        name: "FK_ComunicadoVersaoLog_ComunicadoVersao_IdComunicadoVersao",
                        column: x => x.IdComunicadoVersao,
                        principalTable: "ComunicadoVersao",
                        principalColumn: "IdComunicadoVersao",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calendario_IdTema",
                table: "Calendario",
                column: "IdTema");

            migrationBuilder.CreateIndex(
                name: "IX_ComunicadoVersao_IdComunicado",
                table: "ComunicadoVersao",
                column: "IdComunicado");

            migrationBuilder.CreateIndex(
                name: "IX_ComunicadoVersao_IdTema",
                table: "ComunicadoVersao",
                column: "IdTema");

            migrationBuilder.CreateIndex(
                name: "IX_ComunicadoVersao_IdTemplate",
                table: "ComunicadoVersao",
                column: "IdTemplate");

            migrationBuilder.CreateIndex(
                name: "IX_ComunicadoVersaoLog_IdComunicadoVersao",
                table: "ComunicadoVersaoLog",
                column: "IdComunicadoVersao");

            migrationBuilder.CreateIndex(
                name: "IX_ContatoDiex_IdAssunto",
                table: "ContatoDiex",
                column: "IdAssunto");

            migrationBuilder.CreateIndex(
                name: "IX_Indicador_IdTipoIndicador",
                table: "Indicador",
                column: "IdTipoIndicador");

            migrationBuilder.CreateIndex(
                name: "IX_Publicacao_IdTema",
                table: "Publicacao",
                column: "IdTema");

            migrationBuilder.CreateIndex(
                name: "IX_Publicacao_IdTipoDeConteudo",
                table: "Publicacao",
                column: "IdTipoDeConteudo");

            migrationBuilder.CreateIndex(
                name: "IX_TemaRelacionamento_IdTema",
                table: "TemaRelacionamento",
                column: "IdTema");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calendario");

            migrationBuilder.DropTable(
                name: "ComunicadoVersaoLog");

            migrationBuilder.DropTable(
                name: "ContatoDiex");

            migrationBuilder.DropTable(
                name: "Publicacao");

            migrationBuilder.DropTable(
                name: "TemaRelacionamento");

            migrationBuilder.DropTable(
                name: "ComunicadoVersao");

            migrationBuilder.DropTable(
                name: "Comunicado");

            migrationBuilder.DropTable(
                name: "Tema");

            migrationBuilder.DropTable(
                name: "Indicador");

            migrationBuilder.DropTable(
                name: "TipoIndicador");
        }
    }
}
