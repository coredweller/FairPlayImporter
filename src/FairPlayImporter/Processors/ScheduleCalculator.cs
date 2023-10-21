using FairPlayImporter.Model;
using System.Globalization;

namespace FairPlayImporter.Processors
{
    public interface ICalculateSchedules
    {
        TaskSchedule GenerateSchedule(string cardName, long playerTaskId, string? notes, Cadence cadence, string when);
    }

    public class ScheduleCalculator : ICalculateSchedules
    {
        public TaskSchedule GenerateSchedule(string cardName, long playerTaskId, string? notes, Cadence cadence, string when)
        {
            var schedule = new TaskSchedule { PlayerTaskId = playerTaskId, Notes = notes };

            switch(cadence)
            {
                case Cadence.Once:
                case Cadence.Unknown:
                case Cadence.AsNeeded:
                    schedule.CronSchedule = ""; //None needed
                    break;
                case Cadence.SemiYearly:
                    //First day of the desired month then 6 months after that
                    var month = ParseMonth(cardName, when);
                    schedule.CronSchedule = $"0 0 1 {month}/6 *";
                    break;
                case Cadence.Quarterly:
                    //At 00:00 on day-of-month 1 in every 3rd month
                    var month2 = ParseMonth(cardName, when);
                    schedule.CronSchedule = $"0 0 1 {month2}/3 *";
                    break;
                case Cadence.Yearly:
                    //First day of the desired month
                    var month3 = ParseMonth(cardName, when);
                    schedule.CronSchedule = $"0 0 1 {month3} *";
                    break;
                case Cadence.Monthly:
                    //Monthly on the same day
                    var day = GetNumbers(cardName, when);
                    schedule.CronSchedule = $"0 0 {day} * *";
                    break;
                case Cadence.Weekly:
                    //Weekly on the same week day
                    var weekDayName = GetCronDayName(cardName, when);
                    schedule.CronSchedule = $"0 0 * * {weekDayName}";
                    break;
                case Cadence.Daily:
                    schedule.CronSchedule = ""; //No cron b/c it happens every day regardless
                    break;
            }

            return schedule;
        }

        private string GetCronDayName(string cardName, string dayOfWeek)
        {
            return dayOfWeek.ToLower() switch
            {
                var str when !string.IsNullOrWhiteSpace(str) => str.Substring(0, 3).ToUpper(),
                _ => throw new ArgumentException($"Invalid day of week: {dayOfWeek} for cardName: {cardName}")
            };
        }

        private int ParseMonth(string cardName, string monthName)
        {
            try
            {
                return DateTime.ParseExact(monthName, "MMMM", CultureInfo.CurrentCulture).Month;
            }
            catch(Exception ex) {
                throw new ArgumentException($"Invalid monthName: {monthName} for cardName: {cardName} with error: {ex}");
            }
        }

        private string GetNumbers(string cardName, string input)
        {
            try
            {
                return new string(input.Where(char.IsDigit).ToArray());
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Invalid day of month: {input} for cardName: {cardName} with error: {ex}");
            }
        }
    }
}
