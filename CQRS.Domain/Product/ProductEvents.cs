using CQRS.Core;
using System;

namespace CQRS.Domain
{

    public class ProductCreatedEvent : BaseEvent
    {
        public readonly string Name;

        public ProductCreatedEvent(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class ProductRenamedEvent : BaseEvent
    {
        public readonly string NewName;

        public ProductRenamedEvent(Guid id, string newName)
        {
            Id = id;
            NewName = newName;
        }
    }

    public class ProductRemovedEvent : BaseEvent
    {
        public ProductRemovedEvent(Guid id)
        {
            Id = id;
        }
    }
}

