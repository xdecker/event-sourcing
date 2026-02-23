using System;
using Common.Core.Events;

namespace Ticketing.Query.Domain.Abstraction;

public interface IEventHandler
{
    Task On(TicketCreatedEvent @event);

    Task On(TicketUpdatedEvent @event);
}
