using CQRS.Core;
using System;

namespace CQRS.Domain
{
    public class Product : AggregateRoot<IEvent>
    {
        private string name;

        public Product() : base(Guid.Empty)
        {
        }

        public Product(Guid id, string name) : base(id)
        {
            this.name = name;

            ApplyChange(new ProductCreatedEvent(Id, name));
        }

        public void ChangeName(string newName)
        {
            if (string.IsNullOrEmpty(newName)) throw new ArgumentException("Product name must be provided");
            ApplyChange(new ProductRenamedEvent(Id, newName));
        }

        public void Remove()
        {
            ApplyChange(new ProductRemovedEvent(Id));
        }
    }

}
