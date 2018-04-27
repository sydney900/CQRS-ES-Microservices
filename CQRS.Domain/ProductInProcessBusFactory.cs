using CQRS.Core;

namespace CQRS.Domain
{
    public class ProductInProcessBusFactory
    {
        public static InProcessBus Create()
        {
            var bus = new InProcessBus();

            var storage = new EventStore(bus);
            var rep = new Repository<Product, IEvent>(storage);

            //register command handlers
            var commands = new ProductCommandHandlers(rep);
            bus.RegisterHandler<CreateProductCommand>(commands.Handle);
            bus.RegisterHandler<RemoveProductCommand>(commands.Handle);
            bus.RegisterHandler<RenameProductCommand>(commands.Handle);

            //register event handlers
            var detail = new ProductDetailEventHandlers();
            bus.RegisterHandler<ProductCreatedEvent>(detail.Handle);
            bus.RegisterHandler<ProductRenamedEvent>(detail.Handle);
            bus.RegisterHandler<ProductRemovedEvent>(detail.Handle);

            var list = new ProductListEventHandlers();
            bus.RegisterHandler<ProductCreatedEvent>(list.Handle);
            bus.RegisterHandler<ProductRenamedEvent>(list.Handle);

            return bus;
        }
    }
}
