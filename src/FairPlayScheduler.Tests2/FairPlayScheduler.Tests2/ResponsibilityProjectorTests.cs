using FairPlayImporter.Repository;
using FairPlayScheduler.Model;
using FairPlayScheduler.Processors;
using FairPlayScheduler.Repository;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace FairPlayScheduler.Tests;

public class ResponsibilityProjectorTests
{
    //Normally I would mock the repos and data but since this is for fun I am taking a shortcut and using my test db
    public string connectionString = "Server=DESKTOP-R0T8REJ\\SQLEXPRESS;Database=FairPlay;User Id=fairPlayUser;Password=fA1rPLAY39!555;MultipleActiveResultSets=true";
    public IUserRepo UserRepo { get; set; }
    public IPlayerHandRepo PlayerHandRepo { get; set; }
    public IProjectResponsibility ResponsibilityProjector { get; set; }
    public string DefaultUserName = "Dan";

    public ResponsibilityProjectorTests()
    {
        var databaseConfig = new DatabaseConfig { ConnectionString = connectionString };
        UserRepo = new UserRepository(databaseConfig);
        PlayerHandRepo = new PlayerHandRepository(databaseConfig);
        ResponsibilityProjector = new ResponsibilityProjector(UserRepo, PlayerHandRepo);
    }

    [Fact]
    public async Task TestMultipleDays()
    {
        //TODO:
    }

    [Fact]
    public async Task DailySchedulesWork()
    {
        var daily = await ResponsibilityProjector.ProjectResponsibilities(DefaultUserName, new DateTime(2023, 10, 29));
        //One day of responsibilities projected
        Assert.Single(daily);
        //17 responsibilities on this day
        Assert.Equal(17, daily.First().Responsibilities.Where(d => d.CadenceId == (int)Cadence.Daily).Count());
    }

    [Fact]
    public async Task WeeklySundaySchedulesWork()
    {
        var weeklySunday = await ResponsibilityProjector.ProjectResponsibilities(DefaultUserName, new DateTime(2023, 10, 29));
        //One day of responsibilities projected
        Assert.Single(weeklySunday);
        //6 responsibilities on this day
        Assert.Equal(6, weeklySunday.First().Responsibilities.Where(d => d.CadenceId == (int)Cadence.Weekly).Count());
    }

    [Fact]
    public async Task FirstOfTheMonthSchedulesWork()
    {
        var monthly = await ResponsibilityProjector.ProjectResponsibilities(DefaultUserName, new DateTime(2023, 11, 1));
        //One day of responsibilities projected
        Assert.Single(monthly);
        //19 responsibilities on this day
        Assert.Equal(19, monthly.First().Responsibilities.Where(d => d.CadenceId == (int)Cadence.Monthly).Count());
    }

    [Fact]
    public async Task FifteenthOfTheMonthSchedulesWork()
    {
        var monthly = await ResponsibilityProjector.ProjectResponsibilities(DefaultUserName, new DateTime(2023, 11, 15));
        //One day of responsibilities projected
        Assert.Single(monthly);
        //1 responsibilities on this day
        Assert.Single(monthly.First().Responsibilities.Where(d => d.CadenceId == (int)Cadence.Monthly));
    }

    [Fact]
    public async Task QuarterlyScheduleExistsOnJan1()
    {
        var quarterly = await ResponsibilityProjector.ProjectResponsibilities(DefaultUserName, new DateTime(2023, 1, 1));
        //One day of responsibilities projected
        Assert.Single(quarterly);
        //15 responsibilities on this day
        Assert.Equal(15, quarterly.First().Responsibilities.Where(d => d.CadenceId == (int)Cadence.Quarterly).Count());
    }

    [Fact]
    public async Task QuarterlyScheduleExistsOnApril1()
    {
        var quarterly = await ResponsibilityProjector.ProjectResponsibilities(DefaultUserName, new DateTime(2023, 4, 1));
        //One day of responsibilities projected
        Assert.Single(quarterly);
        //16 responsibilities on this day
        Assert.Equal(16, quarterly.First().Responsibilities.Where(d => d.CadenceId == (int)Cadence.Quarterly).Count());
    }

    [Fact]
    public async Task QuarterlySchedulesDontExistForJan2()
    {
        var quarterly = await ResponsibilityProjector.ProjectResponsibilities(DefaultUserName, new DateTime(2023, 1, 2));
        //One day of responsibilities projected
        Assert.Single(quarterly);
        //15 responsibilities on this day
        Assert.Equal(0, quarterly.First().Responsibilities.Count(d => d.CadenceId == (int)Cadence.Quarterly));
    }

    [Fact]
    public async Task SemiYearlyScheduleExistsOnJune1()
    {
        var semiYearly = await ResponsibilityProjector.ProjectResponsibilities(DefaultUserName, new DateTime(2023, 6, 1));
        //One day of responsibilities projected
        Assert.Single(semiYearly);
        //2 responsibilities on this day
        Assert.Equal(2, semiYearly.First().Responsibilities.Where(d => d.CadenceId == (int)Cadence.SemiYearly).Count());
    }
}