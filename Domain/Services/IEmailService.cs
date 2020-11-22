namespace Domain.Services
{
    public interface IEmailService
    {
        void Enviar(string para, string de, string assunto, string corpo);
    }
}
