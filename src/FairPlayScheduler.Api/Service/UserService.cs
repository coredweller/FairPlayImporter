using FairPlayScheduler.Api.Model;
using FairPlayScheduler.Api.Repository;

namespace FairPlayScheduler.Api.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<User?> GetUserByName(string playerName)
        {
            var usersByName = await _userRepo.GetUsersByName(playerName);
            var users = usersByName.OrderByDescending(u => u.UpdatedDate.HasValue ? u.UpdatedDate : u.CreatedDate);
            if (!users.Any()) return null;

            var currentUser = users.First();
            return currentUser;
        }

        public async Task<ToEmailSettings> GetToEmailSettings(long playerTaskId)
        {
            var user = await GetUserByPlayerTaskId(playerTaskId);
            return new ToEmailSettings { ToEmail = user.Email ?? string.Empty, ToName = user.Name ?? string.Empty };
        }

        public async Task<User> GetUserByPlayerTaskId(long playerTaskId)
        {
            return await _userRepo.GetUserByPlayerTaskId(playerTaskId);
        }
    }
}
