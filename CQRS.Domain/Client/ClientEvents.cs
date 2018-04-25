using CQRS.Core;
using System;

namespace CQRS.Domain
{

    public class ClientCreatedEvent : BaseEvent
    {
        public readonly string Name;

        public ClientCreatedEvent(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class ClientRenamedEvent : BaseEvent
    {
        public readonly string NewName;

        public ClientRenamedEvent(Guid id, string newName)
        {
            Id = id;
            NewName = newName;
        }
    }

    public class ClientRemovedEvent : BaseEvent
    {
        public ClientRemovedEvent(Guid id)
        {
            Id = id;
        }
    }
}

