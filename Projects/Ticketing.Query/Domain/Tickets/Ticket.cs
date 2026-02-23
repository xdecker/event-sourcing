using System;
using Ticketing.Query.Domain.Abstraction;
using Ticketing.Query.Domain.Employees;
using Ticketing.Query.Domain.TicketTypes;

namespace Ticketing.Query.Domain.Tickets;

public class Ticket : Entity
{
    public string? Description { get; set; }
    public virtual TicketType? TicketType { get; set; }
    public virtual ICollection<Employee> Employees { get; set; } = [];
    public virtual ICollection<TicketEmployee> TicketEmployees { get; set; } = [];

    public Ticket() { }
    private Ticket(Guid id, string description, TicketType? ticketType) : base(id)
    {
        Description = description;
        TicketType = ticketType;

    }

    public static Ticket Create(
        Guid id,
        TicketType? ticketType,
        string description
    )
    {
        return new Ticket(
            id,
            description,
            ticketType
        );
    }


}
