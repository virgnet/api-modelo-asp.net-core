using Domain.Entities;
using Domain.Queries;
using System;
using System.Collections.Generic;

namespace Domain.Repositories
{
    public interface ITemaRepository
    {
        IEnumerable<TemaQueryResult> Pesquisar(string textoDaBusca);
        IEnumerable<TemaQueryResult> PesquisarTodos();
        TemaQueryResult Buscar(Guid id);
        bool Salvar(Tema obj);
        bool Desativar(Guid id);
        bool Excluir(Guid id);
    }
}
