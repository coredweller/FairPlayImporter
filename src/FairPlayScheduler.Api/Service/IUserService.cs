using FairPlayScheduler.Api.Model;

namespace FairPlayScheduler.Api.Service
{
    public interface IUserService
    {
        Task<User?> GetUserByName(string playerName);
        Task<User> GetUserByPlayerTaskId(long playerTaskId);
        Task<ToEmailSettings> GetToEmailSettings(long playerTaskId);
    }
}