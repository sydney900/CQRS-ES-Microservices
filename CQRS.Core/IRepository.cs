using System;

namespace CQRS.Core
{
    public interface IRepository<T, V> where T : AggregateRoot<V>, new() where V : class, IEvent
    {
        void Save(AggregateRoot<V> aggregate, int expectedVersion);
        T GetById(Guid id);
    }
}
