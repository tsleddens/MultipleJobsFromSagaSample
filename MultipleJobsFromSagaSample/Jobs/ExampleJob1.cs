using MassTransit.Contracts.JobService;

namespace MultipleJobsFromSagaSample.Jobs;

public record ExampleJob1;

public record ExampleJob1SubmitJob : SubmitJob<ExampleJob1>
{
    public Guid JobId { get; init; }
    public ExampleJob1 Job { get; init; } = new ExampleJob1();
}