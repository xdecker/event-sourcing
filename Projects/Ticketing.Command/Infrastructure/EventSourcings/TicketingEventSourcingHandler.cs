using System.Security.Cryptography.X509Certificates;
using Ticketing.Command.Application.Aggregates;
using Ticketing.Command.Domain.Abstracts;

namespace Ticketing.Command.Infrastructure.Eventsourcings;

public class TicketingEventSourcingHandler : IEventSourcingHandler<TicketAggregate>
{

    private readonly IEventStore _eventStore;
    public TicketingEventSourcingHandler(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }
    public async Task<TicketAggregate> GetByIdAsync(string aggregateId, CancellationToken cancellationToken)
    {
        //si no existe aqui se genera, si existe se pone como mayor version
        var aggregate = new TicketAggregate();
        var events = await _eventStore.GetEventsAsync(aggregateId, cancellationToken);
        if (events is null || !events.Any())
        {
            return aggregate;
        }
        aggregate.ReplayEvents(events);
        aggregate.Version = events.Select(x => x.Version).Max();
        return aggregate;
    }

    public async Task SaveAsync(AggregateRoot aggregate, CancellationToken cancellationToken)
    {
        await _eventStore.SaveEventsAsync(
            aggregate.Id,
            aggregate.GetUnconmmittedChanges(),
            aggregate.Version, cancellationToken);

        aggregate.MarkChangesAsCommitted();
    }
}