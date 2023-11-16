using FairPlayScheduler.Api.Model;
using FairPlayScheduler.Api.Repository;

namespace FairPlayScheduler.Api.Service
{
    public class CompletedTaskService : ICompletedTaskService
    {
        private readonly ICompletedTaskRepository _taskRepository;

        public CompletedTaskService(ICompletedTaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<CompletedTask> SaveCompletedTask(CompletedTask task)
        {
            return await _taskRepository.InsertCompletedTask(task);
        }
    }
}
