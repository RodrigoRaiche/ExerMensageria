using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using musica_mensageria_consumer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();