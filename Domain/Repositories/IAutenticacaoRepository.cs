using Domain.Queries;

namespace Domain.Repositories
{
    public interface IAutenticacaoRepository
    {
        public AutenticacaoQueryResult Autenticar(string token);
    }
}
