using FairPlayScheduler.Api.Model;
using FairPlayScheduler.Api.Repository;
using FairPlayScheduler.Api.Service;
using NCrontab;

namespace FairPlayScheduler.Processors
{
    public interface IProjectResponsibility
    {
        Task<IList<ResponsibilityByDay>> ProjectResponsibilities(string userName, DateTime startDate, int howManyDays = 1);
    }

    public class ResponsibilityProjector : IProjectResponsibility
    {
        private readonly IUserService _userService;
        private readonly IPlayerHandRepo _playerHandRepo;

        public ResponsibilityProjector(IUserService userService, IPlayerHandRepo playerHandRepo)
        {
            _userService = userService;
            _playerHandRepo = playerHandRepo;
        }

        public async Task<IList<ResponsibilityByDay>> ProjectResponsibilities(string userName, DateTime startDate, int howManyDays = 1)
        {
            var currentUser = await _userService.GetUserByName(userName);
            if(currentUser == null) return new List<ResponsibilityByDay>();

            var responsibilities = await _playerHandRepo.GetResponsibilitiesAsync(currentUser.Id);
            if (!responsibilities.Any()) return new List<ResponsibilityByDay>();

            var output = ProcessResponsibilities(responsibilities, startDate, howManyDays);
            return output;
        }

        //TODO: only mark a responsibility as complete if the completed date matches the projected day
        private IList<ResponsibilityByDay> ProcessResponsibilities(IList<Responsibility> responsibilities, DateTime startDate, int howManyDays)
        {
            var list = GetInitializedList(startDate, howManyDays);
            var output = responsibilities.Select(r =>
            {
                if (CadenceToSkip(r.Cadence)) return r;

                //If its daily then always include it
                if (r.Cadence == Cadence.Daily)
                {
                    list.ForEach(l => {
                        if (r.CompletedDate.HasValue && l.Date.Date == r.CompletedDate.Value.Date) r.MarkAsComplete = true;
                        l.Responsibilities.Add(r);
                    });
                    return r;
                }

                for (int i = 0; i < howManyDays; i++)
                {
                    //https://stackoverflow.com/questions/8121374/calculate-cron-next-run-time-in-c-sharp
                    var schedule = CrontabSchedule.Parse(r.CronSchedule);
                    var currentDate = startDate.AddDays(i-1);
                    var nextOccurrence = schedule.GetNextOccurrence(currentDate);
                    if (nextOccurrence == currentDate.AddDays(1))
                    {
                        if (r.CompletedDate.HasValue && list[i].Date.Date == r.CompletedDate.Value.Date) r.MarkAsComplete = true;
                        list[i].Responsibilities.Add(r);
                    }
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
                list.Add(new ResponsibilityByDay { Date = startDate.AddDays(i) });
            }
            return list;
        }
    }
}
