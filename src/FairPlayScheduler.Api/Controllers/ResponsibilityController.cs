using FairPlayScheduler.Api.Model;
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

        public ResponsibilityController(ILogger<ResponsibilityController> logger, IProjectResponsibility responsibilityProjector)
        {
            _logger = logger;
            _projector = responsibilityProjector;
        }

        [HttpGet(Name = "GetResponsibilitiesForToday")]
        public async Task<IEnumerable<ResponsibilityByDay>> Get()
        {
            var playerName = "Dan"; //Hardcode for testing for now
            var days = 1; //Hardcode for testing for now
            var output = await _projector.ProjectResponsibilities(playerName, DateTime.Now.Date, days);
            return output;
        }
    }
}