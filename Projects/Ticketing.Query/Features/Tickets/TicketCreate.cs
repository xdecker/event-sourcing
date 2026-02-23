using System;
using MediatR;
using Ticketing.Query.Domain.Abstraction;
using Ticketing.Query.Domain.Employees;
using Ticketing.Query.Domain.Tickets;
using Ticketing.Query.Domain.TicketTypes;

namespace Ticketing.Query.Features.Tickets;

public class TicketCreate
{
    public record TicketCreateCommand(
        string Id,
        string Username,
        int TicketType,
        string DetailError

    ) : IRequest<string>;

    public class TicketCreateCommandHandler : IRequestHandler<TicketCreateCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketCreateCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(TicketCreateCommand request, CancellationToken cancellationToken)
        {
            // 1. INSERTAR DATA DE EMPLOYEE
            var employee = await _unitOfWork
            .EmployeeRepository.GetByUsernameAsync(request.Username);
            if (employee is null)
            {
                employee = Employee.Create(
                    string.Empty, string.Empty, null!, request.Username
                );
                _unitOfWork.EmployeeRepository.AddEntity(employee);
            }

            // 2. insertar data de ticket
            var ticket = Ticket.Create(
                new Guid(request.Id),
                TicketType.Create(request.TicketType),
                request.DetailError
            );

            _unitOfWork.RepositoryGeneric<Ticket>().AddEntity(ticket);

            // 3. asignar ticket a empleado TicketEmployee
            var ticketEmployee = TicketEmployee.Create(ticket, employee);
            _unitOfWork.RepositoryGeneric<TicketEmployee>().AddEntity(ticketEmployee);

            await _unitOfWork.Complete();

            return ticket.Id.ToString();
        }
    }
}
