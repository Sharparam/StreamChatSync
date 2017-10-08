namespace Sharparam.StreamChatSync.Twitch
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTwitchServices(this IServiceCollection services)
        {
            return services.AddTransient<ITwitchClient, TwitchClient>();
        }
    }
}
