using CQRS.Core;

namespace CQRS.Domain
{
    public class ClientInProcessBusFactory
    {
        public static InProcessBus Create()
        {
            var bus = new InProcessBus();

            var storage = new EventStore(bus);
            var rep = new Repository<Client, IEvent>(storage);

            //register command handlers
            var commands = new ClientCommandHandlers(rep);
            bus.RegisterHandler<CreateClientCommand>(commands.Handle);
            bus.RegisterHandler<RemoveClientCommand>(commands.Handle);
            bus.RegisterHandler<RenameClientCommand>(commands.Handle);

            //register event handlers
            var detail = new ClientDetailEventHandlers();
            bus.RegisterHandler<ClientCreatedEvent>(detail.Handle);
            bus.RegisterHandler<ClientRenamedEvent>(detail.Handle);
            bus.RegisterHandler<ClientRemovedEvent>(detail.Handle);

            var list = new ClientListEventHandlers();
            bus.RegisterHandler<ClientCreatedEvent>(list.Handle);
            bus.RegisterHandler<ClientRenamedEvent>(list.Handle);

            return bus;
        }
    }
}
