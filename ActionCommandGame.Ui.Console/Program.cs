using System;
using System.IO;
using ActionCommandGame.Repository.Core;
using ActionCommandGame.Services;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;



namespace ActionCommandGame.Ui.ConsoleApp
{
    class Program
    {
        private static IServiceProvider ServiceProvider { get; set; }
        private static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            //var database = ServiceProvider.GetRequiredService<ActionButtonGameDbContext>();
           // database.Initialize();
            
            var game = ServiceProvider.GetRequiredService<Game>();
            game.Start();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            var appSettings = new AppSettings();
            Configuration.Bind(nameof(AppSettings), appSettings);

            services.AddSingleton(appSettings);

            // Register the EntityFramework database using SQL Server
            services.AddDbContext<ActionButtonGameDbContext>(options =>
            {
                options.UseMySQL(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddTransient<Game>();

            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IPositiveGameEventService, PositiveGameEventService>();
            services.AddTransient<INegativeGameEventService, NegativeGameEventService>();
            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<IPlayerService, PlayerService>();
            services.AddTransient<IPlayerItemService, PlayerItemService>();
        }
    }
}