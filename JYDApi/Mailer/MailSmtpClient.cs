using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace JYD.Mailer
{
    public class MailSmtpClient : IMailSmtpClient
    {
        private readonly IOptions<MailSmtp> _config;
        public MailSmtpClient(IOptions<MailSmtp> config)
        {
            _config = config;
        }
        public SmtpClient Smtp()
        {
            return new SmtpClient(_config.Value.Host, _config.Value.Port)
            {
                UseDefaultCredentials = false,
                Credentials = string.IsNullOrEmpty(_config.Value.Password) ? new NetworkCredential() : new NetworkCredential(_config.Value.Username, _config.Value.Password),
                EnableSsl = _config.Value.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
        }
    }
}
