namespace Sharparam.StreamChatSync.Service
{
    using System;

    using Configuration;

    using JetBrains.Annotations;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using PeterKottas.DotNetCore.WindowsService;
    using PeterKottas.DotNetCore.WindowsService.Interfaces;

    [UsedImplicitly]
    internal sealed class ServiceStarter : IServiceStarter
    {
        private readonly ILogger _log;

        private readonly IServiceProvider _services;

        private readonly ServiceConfiguration _config;

        public ServiceStarter(ILogger<ServiceStarter> logger, IServiceProvider services, IOptions<ServiceConfiguration> options)
        {
            _log = logger;
            _services = services;
            _config = options.Value;
        }

        public void Start()
        {
            ServiceRunner<IMicroService>.Run(config =>
            {
                config.SetName(_config.Name);
                config.SetDescription(_config.Description);
                config.SetDisplayName(_config.DisplayName);

                config.Service(service =>
                {
                    service.ServiceFactory((args, controller) => _services.GetRequiredService<IMicroService>());

                    service.OnStart((svc, args) =>
                    {
                        _log.LogInformation("Starting service");
                        svc.Start();
                    });

                    service.OnStop(svc =>
                    {
                        _log.LogInformation("Stopping service");
                        svc.Stop();
                    });

                    service.OnError(exception =>
                    {
                        _log.LogCritical(exception, "Service failed");
                    });
                });
            });
        }
    }
}
