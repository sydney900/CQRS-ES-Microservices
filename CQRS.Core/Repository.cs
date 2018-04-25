using System;

namespace CQRS.Core
{
    public class Repository<T, V> : IRepository<T, V> where T : AggregateRoot<V>, new() where V : class, IEvent
    {
        private readonly IEventStore<V> _storage;

        public Repository(IEventStore<V> storage)
        {
            _storage = storage;
        }

        public void Save(AggregateRoot<V> aggregate, int expectedVersion)
        {
            _storage.Save(aggregate.Id, aggregate.GetUncommittedChanges(), expectedVersion);
        }

        public T GetById(Guid id)
        {
            var obj = new T();//lots of ways to do this
            var e = _storage.Get(id);
            obj.LoadsFromHistory(e);
            return obj;
        }
    }

}
