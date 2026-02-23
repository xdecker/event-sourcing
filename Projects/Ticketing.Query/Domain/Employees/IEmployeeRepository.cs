using System;
using Ticketing.Query.Domain.Abstraction;

namespace Ticketing.Query.Domain.Employees;

public interface IEmployeeRepository : IGenericRepository<Employee>
{
    Task<Employee?> GetByUsernameAsync(string username);
}
