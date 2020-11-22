using Domain.Repositories;
using Infrastructure.DataContext;

namespace Infrastructure.Repositories
{
    public class AutenticacaoRepository : IAutenticacaoRepository
    {
        private readonly ModeloDataContext _context;

        public AutenticacaoRepository(ModeloDataContext context)
        {
            _context = context;
        }
    }
}
