using FairPlayScheduler.Api.Model;

namespace FairPlayScheduler.Api.Service
{
    public interface IMailService
    {
        Task<bool> SendMailAsync(MailData mailData);
        bool SendMail(MailData mailData);
    }
}
