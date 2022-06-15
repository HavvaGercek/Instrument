using System.Net;
using System.Net.Mail;

namespace Instrument.Mail.Worker.Services
{
    public class MailService : IMailService
    {
        public void Send(string to, string subject, string html)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("yourmailaddress", "password"),//change this
                EnableSsl = true,
            };

            smtpClient.Send("yourmailaddress", to, subject, html);
        }
    }
}
