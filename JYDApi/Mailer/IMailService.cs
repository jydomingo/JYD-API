namespace JYD.Mailer
{
    public interface IMailService
    {
        Task<bool> SendTemporaryPassword(string userId, string email, string temporaryPassword, string recipientName = "");
    }
}
