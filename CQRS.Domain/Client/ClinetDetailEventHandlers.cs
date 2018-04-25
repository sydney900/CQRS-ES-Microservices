using CQRS.Core;
using System;

namespace CQRS.Domain
{
    public class ClinetDetailEventHandlers : IHandler<ClientCreatedEvent>, IHandler<ClientRenamedEvent>, IHandler<ClientRemovedEvent>
    {
        public void Handle(ClientCreatedEvent message)
        {
            ClientMemoryDatabase.details.Add(message.Id, new ClientDetailDto(message.Id, message.Name, 1));
        }

        public void Handle(ClientRenamedEvent message)
        {
            ClientDetailDto d = GetDetailsItem(message.Id);
            d.Name = message.NewName;
            d.Version = message.Version;
        }

        private ClientDetailDto GetDetailsItem(Guid id)
        {
            ClientDetailDto d;

            if (!ClientMemoryDatabase.details.TryGetValue(id, out d))
            {
                throw new InvalidOperationException("did not find the client");
            }

            return d;
        }

        public void Handle(ClientRemovedEvent message)
        {
            ClientDetailDto d = GetDetailsItem(message.Id);
            d.Version = message.Version;
        }
    }
}
