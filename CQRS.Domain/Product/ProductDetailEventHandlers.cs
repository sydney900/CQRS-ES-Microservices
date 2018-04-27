using CQRS.Core;
using System;

namespace CQRS.Domain
{
    public class ProductDetailEventHandlers : IHandler<ProductCreatedEvent>, IHandler<ProductRenamedEvent>, IHandler<ProductRemovedEvent>
    {
        public void Handle(ProductCreatedEvent message)
        {
            ProductMemoryDatabase.details.Add(message.Id, new ProductDetailDto(message.Id, message.Name, 1));
        }

        public void Handle(ProductRenamedEvent message)
        {
            ProductDetailDto d = GetDetailsItem(message.Id);
            d.Name = message.NewName;
            d.Version = message.Version;
        }

        private ProductDetailDto GetDetailsItem(Guid id)
        {
            ProductDetailDto d;

            if (!ProductMemoryDatabase.details.TryGetValue(id, out d))
            {
                throw new InvalidOperationException("did not find the Product");
            }

            return d;
        }

        public void Handle(ProductRemovedEvent message)
        {
            ProductDetailDto d = GetDetailsItem(message.Id);
            d.Version = message.Version;
        }
    }
}
