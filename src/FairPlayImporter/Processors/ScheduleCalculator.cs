using FairPlayImporter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayImporter.Processors
{
    public interface ICalculateSchedules
    {
        TaskSchedule GenerateSchedule(long playerTaskId, string? notes, Cadence cadence, string when);
    }

    public class ScheduleCalculator : ICalculateSchedules
    {
        public TaskSchedule GenerateSchedule(long playerTaskId, string? notes, Cadence cadence, string when)
        {
            var schedule = new TaskSchedule { PlayerTaskId = playerTaskId, Notes = notes };
            
            switch(cadence)
            {
                case Cadence.Once:
                case Cadence.Unknown:
                case Cadence.AsNeeded:
                    schedule.CronSchedule = "";
                    break;
                case Cadence.SemiYearly:
                    //TODO: put multiple crons together here
                    break;
                case Cadence.Quarterly:
                    //TODO: put multiple crons together here
                    break;
                case Cadence.Yearly:
                    //TODO: single cron
                    break;
                case Cadence.Monthly:
                    //TODO: single cron
                    break;
                case Cadence.Weekly:
                    //TODO: single cron
                    break;
                case Cadence.Daily:
                    //TODO: single cron
                    break;
            }


            return schedule;
        }
    }
}
