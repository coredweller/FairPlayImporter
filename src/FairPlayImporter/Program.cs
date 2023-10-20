// See https://aka.ms/new-console-template for more information
using FairPlayImporter.Processors;
using FairPlayImporter.Repository;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddScoped<IUserRepo, UserRepository>()
            .AddScoped<IImportDecks, DeckImporter>()
            .AddScoped<ICalculateSchedules, ScheduleCalculator>()
            .BuildServiceProvider();

        Console.WriteLine("Player's Name:");
        var playerName = Console.ReadLine();

        Console.WriteLine("CSV Location:");
        var location = Console.ReadLine();

        var inputValidated = false;
        var message = "";
        switch(playerName, location)
        {
            case ("", ""):
                message = "Please input Name and Location!";
                break;
            case ("", _):
                message = "Please input Name!";
                break;
            case (_, ""):
                message = "Please input Location!";
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

        var importer = serviceProvider.GetService<IImportDecks>();
        if(importer == null) { Console.WriteLine("Importer Missing!"); return; }

        var results = await importer.Import(playerName, location);


        //REPORT RESULTS TO USER
        //how many cards imported
        //how many tasks imported



        //THEN write another application that goes through all of the TaskSchedule's
        // lay out the next week of things by day
        // https://stackoverflow.com/questions/8121374/calculate-cron-next-run-time-in-c-sharp
    }
}