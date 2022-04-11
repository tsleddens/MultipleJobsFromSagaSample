using MassTransit.Contracts.JobService;

namespace MultipleJobsFromSagaSample.Jobs;

public record ExampleJob2;

public record ExampleJob2SubmitJob : SubmitJob<ExampleJob2>
{
    public Guid JobId { get; init; }
    public ExampleJob2 Job { get; init; } = new ExampleJob2();
}