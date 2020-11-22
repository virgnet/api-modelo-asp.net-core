using Shared.FluentValidator;
using Shared.FluentValidator.Validation;
using System;

namespace Domain.Entities
{
    public class Acesso : Notifiable
    {
        public Guid? IdAcesso { get; private set; }
        public string Login { get; private set; }
        public string Senha { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public bool Ativo { get; private set; }

        public Acesso()
        {

        }

        public Acesso(Guid? idAcesso, string login, string senha)
        {
            if (idAcesso == Guid.Empty || idAcesso == null)
                IdAcesso = Guid.NewGuid();
            else
                IdAcesso = IdAcesso.Value;

            Login = login;
            Senha = senha;
            Ativo = true;
            DataCadastro = DateTime.Now;

            AddNotifications(new ValidationContract()
                .HasMaxLen(Login, 50, "Login", "Limite máximo de 50 caracteres.")
                .IsNotNullOrEmpty(Login, "Login", "Campo vazio.")
                .HasMaxLen(Senha, 50, "Senha", "Limite máximo de 50 caracteres.")
                .IsNotNullOrEmpty(Senha, "Senha", "Campo vazio.")
            );
        }
    }
}
