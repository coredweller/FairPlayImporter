using FairPlayScheduler.Api.Model;

namespace FairPlayScheduler.Api.Service
{
    public interface ICompletedTaskService
    {
        Task<CompletedTask> SaveCompletedTask(CompletedTask task);
        Task<IList<CompletedTask>> GetCompletedTasksByUser(string userName, int howManyDays = 1);
    }
}