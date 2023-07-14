using System.Net.Mail;

namespace JYD.Mailer
{
    public interface IMailSmtpClient
    {
        SmtpClient Smtp();
    }
}
