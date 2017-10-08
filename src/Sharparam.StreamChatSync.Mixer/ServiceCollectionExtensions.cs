namespace Sharparam.StreamChatSync.Mixer
{
    using Api;

    using Microsoft.Extensions.DependencyInjection;

    using Rest;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixerServices(this IServiceCollection services)
        {
            return services.AddTransient<IRestClient, RestClient>()
                .AddTransient<IMixerApi, MixerApi>()
                .AddTransient<IMixerClient, MixerClient>();
        }
    }
}
