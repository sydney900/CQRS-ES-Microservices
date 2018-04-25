using CQRS.Core;

namespace CQRS.Domain
{
    public class ClientListEventHandlers : IHandler<ClientCreatedEvent>, IHandler<ClientRenamedEvent>, IHandler<ClientRemovedEvent>
    {
        public void Handle(ClientCreatedEvent message)
        {
            ClientMemoryDatabase.list.Add(new ClientListDto(message.Id, message.Name));
        }

        public void Handle(ClientRenamedEvent message)
        {
            var item = ClientMemoryDatabase.list.Find(x => x.Id == message.Id);
            item.Name = message.NewName;
        }

        public void Handle(ClientRemovedEvent message)
        {
            ClientMemoryDatabase.list.RemoveAll(x => x.Id == message.Id);
        }
    }
}
