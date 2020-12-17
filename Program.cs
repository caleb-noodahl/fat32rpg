using MerchantRPG.Models.Configuration;
using MerchantRPG.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MerchantRPG
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var provider = services.BuildServiceProvider();
            await provider.GetService<App>().Run(); 
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath($"{Directory.GetCurrentDirectory()}/Config/")
                .AddJsonFile("app-settings.json", false)
                .Build();

            services.AddOptions();
            services.Configure<EngineSettings>(configuration.GetSection("EngineSettings"));
            services.Configure<PlayerSettings>(configuration.GetSection("PlayerSettings"));
            services.Configure<GameEventSettings>(configuration.GetSection("GameEventSettings"));
            services.Configure<InventorySettings>(configuration.GetSection("InventorySettings"));
            services.Configure<TownSettings>(configuration.GetSection("TownSettings"));

            services.AddScoped<IEventService, EventService>(); 
            services.AddScoped<IGameEngineService, GameEngineService>();

            services.AddSingleton<App>(); 

        }
    }   
}
