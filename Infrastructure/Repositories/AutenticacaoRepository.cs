using Domain.Queries;
using Domain.Repositories;
using Infrastructure.DataContext;
using Infrastructure.Query;

namespace Infrastructure.Repositories
{
    public class AutenticacaoRepository : IAutenticacaoRepository
    {
        private readonly ModeloDataContext _context;

        public AutenticacaoRepository(ModeloDataContext context)
        {
            _context = context;
        }

        public AutenticacaoQueryResult Autenticar(string token)
        {
            return AutenticacaoQuery.Autenticar(_context.ConnectionString, token);
        }
    }
}
