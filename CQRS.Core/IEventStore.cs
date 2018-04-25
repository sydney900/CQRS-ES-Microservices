using System;
using System.Collections.Generic;

namespace CQRS.Core
{
    public interface IEventStore<T> where T: class
    {
        void Save(Guid aggregateId, IEnumerable<T> events, int expectedVersion);
        List<T> Get(Guid aggregateId);
    }
}
