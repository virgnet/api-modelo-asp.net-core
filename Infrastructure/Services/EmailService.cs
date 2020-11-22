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
            MailAddress from = new MailAddress("noreply@xxx.org.br", "Fale com a gente"); // E-mail de remetente cadastrado no painel
            string user = "noreply@xxx.org.br"; // Usuário de autenticação do servidor SMTP
            string pass = "???"; // Senha de autenticação do servidor SMTP

            MailMessage message = new MailMessage(from, new MailAddress(para));

            message.Headers.Add("Message-Id", "<" + Guid.NewGuid().ToString() + "@xxx.org.br>");
            message.Subject = assunto;
            message.IsBodyHtml = true;
            message.Body = corpo;

            using (SmtpClient smtp = new SmtpClient("xxx.xxx.com.br", 587))
            {
                smtp.Credentials = new NetworkCredential(user, pass);
                smtp.Send(message);
            }
        }
    }
}
