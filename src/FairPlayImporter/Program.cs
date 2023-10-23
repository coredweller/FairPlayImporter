// See https://aka.ms/new-console-template for more information
using FairPlayImporter.Processors;
using FairPlayImporter.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    public static async Task Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false)
        .Build();

        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddScoped<IUserRepo, UserRepository>()
            .AddScoped<ICardRepo, CardRepository>()
            .AddScoped<IScheduleRepo, ScheduleRepository>()
            .AddScoped<IImportDecks, DeckImporter>()
            .AddScoped<IPersistPlayerHands, PlayerHandPersister>()
            .AddScoped<ICalculateSchedules, ScheduleCalculator>()
            .AddSingleton(configuration)
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
            case (null, null):
                message = "Please input Name and Location!";
                break;
            case ("", _):
            case (null, _):
                message = "Please input Name!";
                break;
            case (_, ""):
            case (_, null):
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

        var playerHand = await importer.Import(playerName, location);

        var persister = serviceProvider.GetService<IPersistPlayerHands>();
        if(persister == null) { Console.WriteLine("Persister Missing!"); return; }

        var results = persister.SavePlayerHand(playerHand);

        Console.WriteLine($"Number of tasks imported: {playerHand.Cards.Count} and Number of tasks saved: {results.Cards.Count}");
    }
}