using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace JYD.Mailer
{
    public class MailService : IMailService
    {
        private readonly IMailSmtpClient _smtpClient;
        private readonly IOptions<MailSmtp> _config;
        public IConfiguration Configuration { get; }
        public MailService(IMailSmtpClient smtpClient, IOptions<MailSmtp> config, IConfiguration configuration)
        {
            _config = config;
            _smtpClient = smtpClient;
            Configuration = configuration;
        }
        public async Task<bool> SendTemporaryPassword(string userId, string email, string temporaryPassword, string recipientName = "")
        {            
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(_config.Value.Username, _config.Value.DisplayName),
                Subject = "SGE - Forgot Password",
                Body = String.Format(@"<div style=""border -style:solid;border-width:thin;border-color:#dadce0;border-radius:8px;padding:40px 20px;"">
                                        <div style=""line-height:32px""> 
                                            <div style =""font-size:18px;"">Dear {0},</div>
                                        </div>
                                        <div style=""font-family:Roboto-Regular, Helvetica, Arial, sans-serif;font-size:14px;color:rgba(0,0,0,0.87);line-height:20px;padding-top:20px;"">
                                            <p>Good day!<br/><br/>This email confirms that you recently requested to reset your password for your {3} account.<br/><br/>You can now login with the temporary password below:</p>
                                            <p><b>User ID: </b>{1}<br/><b>Temporary Password : </b>{2}</p>
                                            <p>Once login, you will need to change the temporary password to access your account. For assistance, please call our Customer Care at 8922.0301.</p>
                                            <p>Thank you and please be guided accordingly.<br/><br/>Sincerely,<br/><br/>-- {3} Team</p>
                                            <p><b>Note:</b> If you are receiving this email by mistake, please disregard and delete this email.</p>
                                            <p>***** This is a system generated message, do not reply *****</p>
                                        </div>
                                       </div>", recipientName, userId, temporaryPassword, Configuration["AppName"]),
                IsBodyHtml = true
            };

            if (!string.IsNullOrEmpty(recipientName))
                mailMessage.To.Add(new MailAddress(email, recipientName));
            else
                mailMessage.To.Add(email);

            await _smtpClient.Smtp().SendMailAsync(mailMessage);

            return true;            
        }
    }
}
