using FairPlayImporter.Model;

namespace FairPlayImporter.Repository
{
    public interface IUserRepo
    {
        Task<IList<User>> GetUsersByName(string name);
    }
}
