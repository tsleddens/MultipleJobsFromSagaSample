using MassTransit;

namespace MultipleJobsFromSagaSample.Requests;

public record ExecuteExampleJob2 : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; init; }
}
