
using FairPlayScheduler.Api.Configuration;
using FairPlayScheduler.Api.Repository;
using FairPlayScheduler.Api.Service;
using FairPlayScheduler.Processors;

namespace FairPlayScheduler.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<HttpResponseExceptionFilter>();
            });

            //Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Configuration
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .Build();

            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

            var connectionString = configuration.GetConnectionString("FairPlayDatabaseContext") ?? throw new ArgumentException("NO CONFIGURATION FOR CONNECTION STRINGS!!");
            var dbConfig = new DatabaseConfig { ConnectionString = connectionString };

            //DI
            builder.Services
                .AddScoped<IUserRepo, UserRepository>()
                .AddScoped<IPlayerHandRepo, PlayerHandRepository>()
                .AddScoped<ICompletedTaskRepository, CompletedTaskRepository>()
                .AddScoped<IProjectResponsibility, ResponsibilityProjector>()
                .AddScoped<ICompletedTaskService, CompletedTaskService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<INotificationService, NotificationService>()
                .AddScoped<ITemplateService, TemplateService>()
                .AddSingleton(configuration)
                .AddSingleton<IDatabaseConfig>(dbConfig)
                .AddTransient<IMailService, MailService>();

            //Cors
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:3000", "http://localhost:4200")
                                            .AllowAnyMethod().AllowAnyHeader();
                                  });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseHttpsRedirection();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}