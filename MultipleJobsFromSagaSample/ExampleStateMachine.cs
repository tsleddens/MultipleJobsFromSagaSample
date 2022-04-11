using MassTransit;
using MassTransit.Contracts.JobService;
using MultipleJobsFromSagaSample.Jobs;
using MultipleJobsFromSagaSample.Requests;

namespace MultipleJobsFromSagaSample;

internal class ExampleStateMachine : MassTransitStateMachine<ExampleState>
{
    public Request<ExampleState, ExampleJob1, JobSubmissionAccepted> ExampleJob1 { get; private set; }
    public Request<ExampleState, ExampleJob2, JobSubmissionAccepted> ExampleJob2 { get; private set; }

    public Event<ExecuteExampleJob1> ExecuteExampleJob1Triggered { get; private set; }
    public Event<ExecuteExampleJob2> ExecuteExampleJob2Triggered { get; private set; }

    public State ExecutingJob1 { get; private set; }
    public State ExecutingJob2 { get; private set; }

    public ExampleStateMachine()
    {
        Request(() => ExampleJob1);
        Request(() => ExampleJob2);

        InstanceState(x => x.CurrentState);

        Initially(
            When(ExecuteExampleJob1Triggered)
                .Then(x => x.Saga.ExampleJob1Id = NewId.NextGuid())
                .Then(x => x.Saga.ExampleJob2Id = NewId.NextGuid())
                .Request(ExampleJob1, x => x.Init<ExampleJob1>(new { JobId = x.Saga.ExampleJob1Id }))
                .TransitionTo(ExampleJob1.Pending)
        );

        During(ExampleJob1.Pending,
            When(ExampleJob1.Completed)
                .TransitionTo(ExecutingJob1)
        );

        During(ExampleJob2.Pending,
            When(ExampleJob2.Completed)
                .TransitionTo(ExecutingJob2)
        );

        DuringAny(
            When(ExecuteExampleJob2Triggered)
                .ThenAsync(CancelExampleJob1)
                .Request(ExampleJob2, x => x.Init<ExampleJob2>(new { JobId = x.Saga.ExampleJob2Id }))
                .TransitionTo(ExampleJob2.Pending)
        );
    }

    private async Task CancelExampleJob1(BehaviorContext<ExampleState, ExecuteExampleJob2> context)
    {
        await context.Publish<CancelJob>(new {JobId = context.Saga.ExampleJob1Id});
    }
}