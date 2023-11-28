using AutoMapper;
using FairPlayScheduler.Api.Model;
using FairPlayScheduler.Api.Model.Api;
using FairPlayScheduler.Api.Service;
using Microsoft.AspNetCore.Mvc;

namespace FairPlayScheduler.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompletedTaskController : ControllerBase
    {
        private readonly ICompletedTaskService _taskService;
        private readonly IMapper _mapper;

        public CompletedTaskController(ICompletedTaskService taskService, IMapper mapper)
        {
            _taskService = taskService;
            _mapper = mapper;
        }

        [HttpPost(Name = "SaveCompletedResponse")]
        public async Task<CompletedTaskResponse> SaveCompletedResponse(CompletedTaskRequest request)
        {
            var task = _mapper.Map<CompletedTask>(request);
            var savedTask = await _taskService.SaveCompletedTask(task);
            var response = _mapper.Map<CompletedTaskResponse>(savedTask);
            return response;
        }

        [HttpGet("/{playerName}/{days}", Name = "GetAllCompletedTasksForUser")]
        public async Task<IList<CompletedTaskResponse>> GetCompletedTasksForUser(string playerName, int days)
        {
            var tasks = await _taskService.GetCompletedTasksByUser(playerName, days);
            var response = _mapper.Map<IList<CompletedTaskResponse>>(tasks);
            return response;
        }
    }
}
