using System;
using Ticketing.Query.Domain.Employees;

namespace Ticketing.Query.Domain.Abstraction;

public interface IUnitOfWork
{
    IEmployeeRepository EmployeeRepository { get; }
    IGenericRepository<TEntity> RepositoryGeneric<TEntity>() where TEntity : class;

    Task<int> Complete();

}
