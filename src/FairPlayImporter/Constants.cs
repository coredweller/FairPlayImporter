namespace FairPlayImporter
{
    public static class Constants
    {
        public static class ScheduleOptions
        {
            public static class YearlyScheduleOptions 
            {
                public static IList<string> AllYearlyScheduleOptions = new List<string> { Jan1st, Feb1st, Mar1st, Apr1st, May1st, Jun1st, Jul1st, Aug1st, Sep1st, Oct1st, Nov1st, Dec1st };

                public const string Jan1st = "January 1st";
                public const string Feb1st = "February 1st";
                public const string Mar1st = "March 1st";
                public const string Apr1st = "April 1st";
                public const string May1st = "May 1st";
                public const string Jun1st = "June 1st";
                public const string Jul1st = "July 1st";
                public const string Aug1st = "August 1st";
                public const string Sep1st = "September 1st";
                public const string Oct1st = "October 1st";
                public const string Nov1st = "November 1st";
                public const string Dec1st = "December 1st";
            }

            public static class MonthlyScheduleOptions
            {
                public static IList<string> AllMonthlyScheduleOptions = new List<string> { FirstOfMonth, FifthOfMonth, TenthOfMonth, FifteenthOfMonth, TwentiethOfMonth, TwentyFifthOfMonth, LastDayOfMonth };

                public const string FirstOfMonth = "1st of month";
                public const string FifthOfMonth = "5th of month";
                public const string TenthOfMonth = "10th of month";
                public const string FifteenthOfMonth = "15th of month";
                public const string TwentiethOfMonth = "20th of month";
                public const string TwentyFifthOfMonth = "25th of month";
                public const string LastDayOfMonth = "Last day of month";
            }

            public static class WeeklyScheduleOptions
            {
                public static IList<string> AllWeeklyScheduleOptions = new List<string> { Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday};

                public const string Sunday = "Sunday";
                public const string Monday = "Monday";
                public const string Tuesday = "Tuesday";
                public const string Wednesday = "Wednesday";
                public const string Thursday = "Thursday";
                public const string Friday = "Friday";
                public const string Saturday = "Saturday";
            }
        }
    }
}
