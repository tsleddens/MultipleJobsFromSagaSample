using MassTransit;
using MultipleJobsFromSagaSample.Jobs;

namespace MultipleJobsFromSagaSample.JobConsumers;

public class ExampleJob2Consumer : IJobConsumer<ExampleJob2>
{
    public async Task Run(JobContext<ExampleJob2> context)
    {
        while (!context.CancellationToken.IsCancellationRequested)
        {
            await Console.Out.WriteAsync($"Executing {nameof(ExampleJob2)}");
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }
    }
}