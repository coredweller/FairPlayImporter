using FairPlayScheduler.Api.Model;

namespace FairPlayScheduler.Api.Service
{
    public interface INotificationService
    {
        Task<bool> SendResponsibilityDigestNotification(EmailAuthorizationSettings settings, ResponsibilityByDay responsibilities);
    }
}