namespace Sharparam.StreamChatSync.Discord
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDiscordServices(this IServiceCollection services)
        {
            return services.AddTransient<IDiscordClient, DiscordClient>();
        }
    }
}
