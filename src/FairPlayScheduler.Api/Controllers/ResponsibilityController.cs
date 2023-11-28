using AutoMapper;
using FairPlayScheduler.Api.Model;
using FairPlayScheduler.Api.Model.Api;
using FairPlayScheduler.Api.Service;
using FairPlayScheduler.Processors;
using Microsoft.AspNetCore.Mvc;

namespace FairPlayScheduler.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResponsibilityController : ControllerBase
    {
        private readonly ILogger<ResponsibilityController> _logger;
        private readonly IProjectResponsibility _projector;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public ResponsibilityController(ILogger<ResponsibilityController> logger, IProjectResponsibility responsibilityProjector,
            IMapper mapper, INotificationService notificationService)
        {
            _logger = logger;
            _projector = responsibilityProjector;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        [HttpGet("{playerName}/{days}", Name = "GetResponsibilitiesForToday")]
        public async Task<IEnumerable<DailyResponsibilityResponse>> Get(string playerName, int days)
        {
            var list = await _projector.ProjectResponsibilities(playerName, DateTime.Now.Date, days);
            var output = _mapper.Map<IEnumerable<DailyResponsibilityResponse>>(list);
            return output;
        }

        [HttpPut("send/digest/{playerName}", Name = "SendResponsibilitiesByEmail")]
        public async Task<SendResponsibilitiesEmailResponse> Send(SendResponsibilitiesEmailRequest request)
        {
            var settings = _mapper.Map<EmailAuthorizationSettings>(request);
            var responsibilities = _mapper.Map<ResponsibilityByDay>(request.ResponsibilitySet);
            var success = await _notificationService.SendResponsibilityDigestNotification(settings, responsibilities);
            return new SendResponsibilitiesEmailResponse {  IsSuccess = success };
        }
    }
}