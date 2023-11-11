using FairPlayScheduler.Api.Model;

namespace FairPlayScheduler.Api.Repository
{
    public interface IUserRepo
    {
        Task<IList<User>> GetUsersByName(string name);
    }
}
