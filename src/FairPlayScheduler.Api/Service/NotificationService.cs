using FairPlayScheduler.Api.Model;

namespace FairPlayScheduler.Api.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IMailService _mailService;
        private readonly ITemplateService _templateService;
        private readonly IUserService _userService;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IMailService mailService, ITemplateService templateService, IUserService userService, ILogger<NotificationService> logger)
        {
            _mailService = mailService;
            _templateService = templateService;
            _userService = userService;
            _logger = logger;
        }

        public async Task<bool> SendResponsibilityDigestNotification(EmailAuthorizationSettings settings, ResponsibilityByDay responsibilities)
        {
            var playerTaskId = responsibilities?.Responsibilities?.First()?.PlayerTaskId;
            if(!playerTaskId.HasValue) 
            {
                _logger.LogError($"PlayerTaskId cannot be found for SendResponsibilityDigestNotification");
                return false;
            }

            var userEmailSettings = await _userService.GetToEmailSettings(playerTaskId.Value);
            var template = _templateService.RenderResponsibilityDigestEmailTemplate(responsibilities);
            var mailData = new MailData
            {
                UserName = settings.UserName,
                Password = settings.Password,
                Body = template,
                Subject = "Here is your Responsibilities digest!",
                ToEmail = userEmailSettings.ToEmail,
                ToName = userEmailSettings.ToName
            };
            var success = await _mailService.SendMailAsync(mailData);
            return success;
        }
    }
}
