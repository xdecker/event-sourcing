using System;

namespace Ticketing.Query.Domain.Abstraction;

public interface IGenericRepository<T> where T : class
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);

    void AddEntity(T Entity);

    void UpdateEntity(T Entity);

    void DeleteEntity(T Entity);
}
