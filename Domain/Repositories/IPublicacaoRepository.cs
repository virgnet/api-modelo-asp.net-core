using Domain.Entities;
using Domain.Queries;
using System;
using System.Collections.Generic;

namespace Domain.Repositories
{
    public interface IPublicacaoRepository
    {
        IEnumerable<PublicacaoQueryResult> PesquisarPublicacao(Guid? idTipoDeConteudo, Guid? idTema, string textoDaBusca);
        PublicacaoQueryResult BuscarPublicacao(Guid idPublicacao);
        bool SalvarPublicacao(Publicacao publicacao);
        bool SalvarArquivos(Publicacao publicacao);
        //Apoio
        IEnumerable<TipoDeConteudoQueryResult> ListarTipoDeConteudo();
    }
}
