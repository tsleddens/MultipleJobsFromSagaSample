using MassTransit;

namespace MultipleJobsFromSagaSample;

public class ExampleState : SagaStateMachineInstance
{
    public string CurrentState { get; set; }

    public Guid ExampleJob1Id { get; set; }
    public Guid ExampleJob2Id { get; set; }

    public Guid CorrelationId { get; set; }
}