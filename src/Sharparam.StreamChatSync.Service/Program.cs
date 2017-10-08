namespace Sharparam.StreamChatSync.Service
{
    using System;
    using System.IO;
    using System.Net.Sockets;

    using Configuration;

    using Discord;
    using Discord.Configuration;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Mixer;
    using Mixer.Configuration;

    using PeterKottas.DotNetCore.WindowsService.Interfaces;

    using Serilog;

    using Twitch;
    using Twitch.Configuration;

    public static class Program
    {
        private static ILogger _log;

        public static void Main()
        {
            var configuration = BuildConfig();

            Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            var collection = new ServiceCollection().AddStreamChatSyncServices(configuration, Log.Logger);

            var provider = collection.BuildServiceProvider();

            _log = Log.ForContext(typeof(Program));

            _log.Information("Initialisation finished");

            _log.Information("Starting service");

            provider.GetRequiredService<IServiceStarter>().Start();

            if (Environment.UserInteractive)
            {
                _log.Information("Interactive mode, press [enter] to exit");
                Console.ReadLine();
            }
        }

        private static IServiceCollection AddStreamChatSyncServices(this IServiceCollection services, IConfiguration configuration, ILogger logger)
        {
            return services.AddOptions()
                .ConfigureStreamChatSync(configuration)
                .AddLogging(c => c.AddSerilog(logger, true))
                .AddSingleton<IServiceStarter, ServiceStarter>()
                .AddTransient<IMicroService, SyncService>()
                .AddTwitchServices()
                .AddMixerServices()
                .AddDiscordServices();
        }

        private static IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appSettings.json", false, true)
                .AddJsonFile("appSettings.local.json", true, true)
                .Build();
        }

        private static IServiceCollection ConfigureStreamChatSync(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure<ServiceConfiguration>(configuration)
                .Configure<DiscordConfiguration>(o => configuration.Bind("Discord", o))
                .Configure<TwitchConfiguration>(o => configuration.Bind("Twitch", o))
                .Configure<MixerConfiguration>(o => configuration.Bind("Mixer", o));
        }
    }
}
