using Domain.Queries;

namespace Domain.Services
{
    public interface IAutenticacaoService
    {
        UsuarioAutenticacaoQueryResult Logar(string login, string senha);
    }
}
