using FairPlayImporter.Repository;
using FairPlayScheduler.Model;
using FairPlayScheduler.Processors;
using FairPlayScheduler.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FairPlayScheduler
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .Build();

            var connectionString = configuration.GetConnectionString("FairPlayDatabaseContext") ?? throw new ArgumentException("NO CONFIGURATION FOR CONNECTION STRINGS!!");
            var dbConfig = new DatabaseConfig { ConnectionString = connectionString };

            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddScoped<IUserRepo, UserRepository>()
            .AddScoped<IPlayerHandRepo, PlayerHandRepository>()
            .AddScoped<IProjectResponsibility, ResponsibilityProjector>()
            .AddSingleton(configuration)
            .AddSingleton<IDatabaseConfig>(dbConfig)
            .BuildServiceProvider();

            Console.WriteLine("Player's Name:");
            var playerName = Console.ReadLine();

            Console.WriteLine("How many days out do you want projected?");
            var daysStr = Console.ReadLine();

            var inputValidated = false;
            var message = "";
            switch(playerName, daysStr)
            {
                case ("", ""):
                case (null, null):
                    message = "Please input Name and Days!";
                    break;
                case ("", _):
                case (null, _):
                    message = "Please input Name!";
                    break;
                case (_, ""):
                case (_, null):
                    message = "Please input Days!";
                    break;
                case (_, _):
                    inputValidated = true;
                    break;
            }

            if (!inputValidated)
            {
                Console.WriteLine(message);
                return;
            }

            if (!int.TryParse(daysStr, out int days))
            {
                Console.WriteLine($"Invalid value for days: {daysStr}");
                return;
            }
            
            var projector = serviceProvider.GetService<IProjectResponsibility>();
            if(projector == null) { Console.WriteLine("Projector Missing!"); return; }

            //TODO: Take the start date in from user
            var output = await projector.ProjectResponsibilities(playerName, DateTime.Now.Date, days);
            output.Select(o =>
            {
                Console.WriteLine($"Date: {o.Date.Date}");

                o.Responsibilities.Select(r => {
                    var cadence = (Cadence)r.CadenceId;
                    Console.WriteLine($"  Card: {r.CardName} Suit: {r.Suit} Type: {r.TaskType} Requirement: {r.Requirement} Minimum Standard: {r.MinimumStandard} Cron Schedule: {r.CronSchedule} When: {r.When} Notes: {r.Notes} Cadence: {cadence}");
                    return r;
                }).ToList();
                return o;
            }).ToList();

            //TODO: Make a better display mechanism
            //TODO: Make a way to check it off and keep track of it in a Database
            //  so you know when you did it last and be able to take a note next to it 
            //  and then you can take it off the list so you can have a running todo list
        }
    }
}