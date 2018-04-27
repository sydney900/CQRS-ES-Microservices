using CQRS.Core;
using System;

namespace CQRS.Domain
{
    public class ProductCommandHandlers
    {
        private readonly IRepository<Product, IEvent> _repository;

        public ProductCommandHandlers(IRepository<Product, IEvent> repository)
        {
            _repository = repository;
        }

        public void Handle(CreateProductCommand message)
        {
            var item = new Product(message.Id, message.Name);
            _repository.Save(item, -1);
        }

        public void Handle(RemoveProductCommand message)
        {
            var item = _repository.GetById(message.Id);
            item.Remove();
            _repository.Save(item, message.OriginalVersion);
        }

        public void Handle(RenameProductCommand message)
        {
            var item = _repository.GetById(message.Id);
            item.ChangeName(message.NewName);
            _repository.Save(item, message.OriginalVersion);
        }
    }
}
