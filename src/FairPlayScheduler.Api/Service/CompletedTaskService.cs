using FairPlayScheduler.Api.Model;
using FairPlayScheduler.Api.Repository;

namespace FairPlayScheduler.Api.Service
{
    public class CompletedTaskService : ICompletedTaskService
    {
        private readonly ICompletedTaskRepository _taskRepository;
        private readonly IUserService _userService;

        public CompletedTaskService(ICompletedTaskRepository taskRepository, IUserService userService)
        {
            _taskRepository = taskRepository;
            _userService = userService;
        }

        public async Task<CompletedTask> SaveCompletedTask(CompletedTask task)
        {
            return await _taskRepository.InsertCompletedTask(task);
        }

        public async Task<IList<CompletedTask>> GetCompletedTasksByUser(string userName, int howManyDays = 1)
        {
            var currentUser = await _userService.GetUserByName(userName);
            if (currentUser == null) return new List<CompletedTask>();

            var completedTasks = await _taskRepository.GetCompletedTasksAsync(currentUser.Id, howManyDays);
            return completedTasks;
        }
    }
}
