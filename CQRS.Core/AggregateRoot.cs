using System;
using System.Collections.Generic;

namespace CQRS.Core
{
    public abstract class AggregateRoot<T> where T: class, IEvent
    {
        private readonly List<T> _changes = new List<T>();

        public Guid Id { get; protected set; }
        public int Version { get; internal set; }

        public AggregateRoot(Guid id)
        {
            Id = id;
        }

        public IEnumerable<T> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void LoadsFromHistory(IEnumerable<T> history)
        {
            foreach (var e in history) ApplyChange(e, false);
        }

        protected void ApplyChange(T @event)
        {
            ApplyChange(@event, true);
        }

        // push atomic aggregate changes to local history for further processing (EventStore.SaveEvents)
        private void ApplyChange(T @event, bool isNew)
        {
            this.AsDynamic().Apply(@event);
            if(isNew) _changes.Add(@event);
        }
    }

}
