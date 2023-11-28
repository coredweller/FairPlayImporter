using FairPlayScheduler.Api.Model;

namespace FairPlayScheduler.Api.Repository
{
    public interface ICompletedTaskRepository
    {
        Task<CompletedTask> InsertCompletedTask(CompletedTask task);
        Task<IList<CompletedTask>> GetCompletedTasksAsync(long userId, int days);
    }
}