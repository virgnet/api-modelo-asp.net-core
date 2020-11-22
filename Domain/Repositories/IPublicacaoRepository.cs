using Domain.Entities;
using Domain.Queries;
using System;
using System.Collections.Generic;

namespace Domain.Repositories
{
    public interface IPublicacaoRepository
    {
        IEnumerable<PublicacaoQueryResult> Pesquisar(Guid? idTema, string textoDaBusca);
        PublicacaoQueryResult Buscar(Guid idPublicacao);
        bool Salvar(Publicacao obj);
        bool Desativar(Publicacao obj);
    }
}
