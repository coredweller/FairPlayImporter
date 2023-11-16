using FairPlayScheduler.Api.Model;

namespace FairPlayScheduler.Api.Service
{
    public interface ICompletedTaskService
    {
        Task<CompletedTask> SaveCompletedTask(CompletedTask task);
    }
}