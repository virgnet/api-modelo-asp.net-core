using Domain.Services;
using System;
using System.Net.Mail;
using System.Net;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public void Enviar(string para, string de, string assunto, string corpo)
        {
            MailAddress from = new MailAddress("noreply@sestsenat.org.br", "Fale com a DIEX"); // E-mail de remetente cadastrado no painel
            string user = "noreply@sestsenat.org.br"; // Usuário de autenticação do servidor SMTP
            string pass = "d9vcnazbsuyx"; // Senha de autenticação do servidor SMTP

            MailMessage message = new MailMessage(from, new MailAddress(para));

            message.Headers.Add("Message-Id", "<" + Guid.NewGuid().ToString() + "@sestsenat.org.br>");
            message.Subject = assunto;
            message.IsBodyHtml = true;
            message.Body = corpo;

            using (SmtpClient smtp = new SmtpClient("smtps.spezi.com.br", 587))
            {
                smtp.Credentials = new NetworkCredential(user, pass);
                smtp.Send(message);
            }
        }
    }
}
