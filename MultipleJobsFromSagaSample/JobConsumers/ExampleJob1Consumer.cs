using MassTransit;
using MultipleJobsFromSagaSample.Jobs;

namespace MultipleJobsFromSagaSample.JobConsumers;

public class ExampleJob1Consumer : IJobConsumer<ExampleJob1>
{
    public async Task Run(JobContext<ExampleJob1> context)
    {
        while (!context.CancellationToken.IsCancellationRequested)
        {
            await Console.Out.WriteAsync($"Executing {nameof(ExampleJob1)}");
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }
    }
}