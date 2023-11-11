using AutoMapper;
using FairPlayScheduler.Api.Model;
using FairPlayScheduler.Api.Model.Api;
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

        public ResponsibilityController(ILogger<ResponsibilityController> logger, IProjectResponsibility responsibilityProjector,
            IMapper mapper)
        {
            _logger = logger;
            _projector = responsibilityProjector;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetResponsibilitiesForToday")]
        public async Task<IEnumerable<DailyResponsibilityModel>> Get()
        {
            var playerName = "Dan"; //TODO: Hardcode for testing for now
            var days = 1; //TODO: Hardcode for testing for now
            var list = await _projector.ProjectResponsibilities(playerName, DateTime.Now.Date, days);
            var output = _mapper.Map<IEnumerable<DailyResponsibilityModel>>(list);
            return output;
        }
    }
}