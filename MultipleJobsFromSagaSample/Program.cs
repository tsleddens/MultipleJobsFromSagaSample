using System.Reflection;
using Hangfire;
using Hangfire.MemoryStorage;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultipleJobsFromSagaSample;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services)  =>
{
    services.AddHangfire(conf => conf.UseMemoryStorage());
    services.AddHangfireServer();

    services.AddMassTransit(conf =>
    {
        conf.AddConsumers(Assembly.GetExecutingAssembly());
        conf.AddSagaStateMachine<ExampleStateMachine, ExampleState>()
            .InMemoryRepository();

        conf.SetKebabCaseEndpointNameFormatter();

        conf.UsingRabbitMq((rabbitContext, rabbitConf) =>
        {
            rabbitConf.Host(context.Configuration.GetConnectionString("RabbitMQ"));

            var options = new ServiceInstanceOptions()
                .SetEndpointNameFormatter(KebabCaseEndpointNameFormatter.Instance);
            rabbitConf.ServiceInstance(options, instance =>
            {
                instance.ConfigureJobServiceEndpoints(js =>
                {
                    js.FinalizeCompleted = true;
                });
                instance.ConfigureEndpoints(rabbitContext);
            });
        });
    });
});

var host = builder.Build();
await host.RunAsync();