using Domain.Entities;
using Domain.Queries;
using System;
using System.Collections.Generic;

namespace Domain.Repositories
{
    public interface ITemaRepository
    {
        IEnumerable<TemaQueryResult> PesquisarTema(string textoDaBusca);
        IEnumerable<TemaQueryResult> PesquisarTodosTemas();
        TemaQueryResult BuscarTema(Guid id);
        bool SalvarTema(Tema tema);
        bool SalvarTemaRelacionamento(TemaRelacionamento relacionamento);
        bool DesativarTema(Guid id);
        bool ExcluirTemaRelacionamento(Guid id);

        //Apoio
        IEnumerable<AreaQueryResult> PesquisarArea(string textoDaBusca);
        IEnumerable<SistemaQueryResult> PesquisarSistema(string textoDaBusca);
        IEnumerable<DocumentoQueryResult> PesquisarDocumento(string textoDaBusca);
        IEnumerable<ProjetoQueryResult> PesquisarProjeto(string textoDaBusca);
    }
}
