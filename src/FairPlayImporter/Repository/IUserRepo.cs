using FairPlayImporter.Model;

namespace FairPlayImporter.Repository
{
    public interface IUserRepo
    {
        Task<List<User>> GetUsersByName(string name);
        Task<User> CreateUser(string name);
        Task<User> UpdateUser(User user);
    }
}
