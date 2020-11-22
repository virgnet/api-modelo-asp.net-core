using Domain.Entities;
using Domain.Queries;
using System;
using System.Collections.Generic;

namespace Domain.Repositories
{
    public interface IPublicacaoTemaRepository
    {
        IEnumerable<PublicacaoTemaQueryResult> Pesquisar(Guid? idPublicacao, Guid? idTema, string textoDaBusca);
        PublicacaoTemaQueryResult Buscar(Guid idPublicacao, Guid idTema);
        bool Salvar(PublicacaoTema obj);
    }
}
