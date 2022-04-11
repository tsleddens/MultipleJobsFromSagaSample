using MassTransit;

namespace MultipleJobsFromSagaSample.Requests;

public record ExecuteExampleJob1 : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; init; }
}
