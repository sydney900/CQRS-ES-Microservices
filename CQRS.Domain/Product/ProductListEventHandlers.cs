using CQRS.Core;

namespace CQRS.Domain
{
    public class ProductListEventHandlers : IHandler<ProductCreatedEvent>, IHandler<ProductRenamedEvent>, IHandler<ProductRemovedEvent>
    {
        public void Handle(ProductCreatedEvent message)
        {
            ProductMemoryDatabase.list.Add(new ProductListDto(message.Id, message.Name));
        }

        public void Handle(ProductRenamedEvent message)
        {
            var item = ProductMemoryDatabase.list.Find(x => x.Id == message.Id);
            item.Name = message.NewName;
        }

        public void Handle(ProductRemovedEvent message)
        {
            ProductMemoryDatabase.list.RemoveAll(x => x.Id == message.Id);
        }
    }
}
