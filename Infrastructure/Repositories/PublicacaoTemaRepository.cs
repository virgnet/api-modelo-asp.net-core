using Domain.Entities;
using Domain.Queries;
using Domain.Repositories;
using Infrastructure.DataContext;
using System;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public class PublicacaoTemaRepository : IPublicacaoTemaRepository
    {
        private readonly ModeloDataContext _context;

        public PublicacaoTemaRepository(ModeloDataContext context)
        {
            _context = context;
        }

        public PublicacaoTemaQueryResult Buscar(Guid idPublicacao, Guid idTema)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PublicacaoTemaQueryResult> Pesquisar(Guid? idPublicacao, Guid? idTema, string textoDaBusca)
        {
            throw new NotImplementedException();
        }

        public bool Salvar(PublicacaoTema obj)
        {
            throw new NotImplementedException();
        }
    }
}
