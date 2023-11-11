using FairPlayScheduler.Api.Model;
using FairPlayScheduler.Api.Repository;
using NCrontab;

namespace FairPlayScheduler.Processors
{
    public interface IProjectResponsibility
    {
        Task<IList<ResponsibilityByDay>> ProjectResponsibilities(string userName, DateTime startDate, int howManyDays = 1);
    }

    public class ResponsibilityProjector : IProjectResponsibility
    {
        private readonly IUserRepo _userRepo;
        private readonly IPlayerHandRepo _playerHandRepo;

        public ResponsibilityProjector(IUserRepo userRepo, IPlayerHandRepo playerHandRepo)
        {
            _userRepo = userRepo;
            _playerHandRepo = playerHandRepo;
        }

        public async Task<IList<ResponsibilityByDay>> ProjectResponsibilities(string userName, DateTime startDate, int howManyDays = 1)
        {
            var usersByName = await _userRepo.GetUsersByName(userName);
            var users = usersByName.OrderByDescending(u => u.UpdatedDate.HasValue ? u.UpdatedDate : u.CreatedDate);
            if (!users.Any()) throw new ArgumentException($"No user with the name of {userName} found in the system");

            var currentUser = users.First();
            var responsibilities = await _playerHandRepo.GetResponsibilitiesAsync(currentUser.Id);
            if (!responsibilities.Any()) throw new ArgumentException($"No responsibilities found for user name: {userName}");

            var output = ProcessResponsibilities(responsibilities, startDate, howManyDays);
            return output;
        }

        private IList<ResponsibilityByDay> ProcessResponsibilities(IList<Responsibility> responsibilities, DateTime startDate, int howManyDays)
        {
            var list = GetInitializedList(startDate, howManyDays);
            var output = responsibilities.Select(r =>
            {
                if (CadenceToSkip(r.Cadence)) return r;

                //If its daily then always include it
                if (r.Cadence == Cadence.Daily)
                {
                    list.ForEach(l => l.Responsibilities.Add(r));
                    return r;
                }

                for (int i = 0; i < howManyDays; i++)
                {
                    //https://stackoverflow.com/questions/8121374/calculate-cron-next-run-time-in-c-sharp
                    var schedule = CrontabSchedule.Parse(r.CronSchedule);
                    var currentDate = startDate.AddDays(i-1);
                    var nextOccurrence = schedule.GetNextOccurrence(currentDate);
                    if (nextOccurrence == currentDate.AddDays(1)) 
                        list[i].Responsibilities.Add(r);
                }

                return r;
            }).ToList();
            return list;
        }

        private bool CadenceToSkip(Cadence cadence)
        {
            if (cadence == Cadence.AsNeeded || cadence == Cadence.Unknown || cadence == Cadence.Once) 
                return true;
            return false;
        }

        private List<ResponsibilityByDay> GetInitializedList(DateTime startDate, int howManyDays)
        {
            var list = new List<ResponsibilityByDay>();

            for (int i = 0; i < howManyDays; i++)
            {
                list.Add(new ResponsibilityByDay(startDate.AddDays(i)));
            }
            return list;
        }
    }
}
