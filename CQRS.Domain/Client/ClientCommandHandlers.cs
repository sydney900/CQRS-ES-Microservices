using CQRS.Core;
using System;

namespace CQRS.Domain
{
    public class ClientCommandHandlers
    {
        private readonly IRepository<Client, IEvent> _repository;

        public ClientCommandHandlers(IRepository<Client, IEvent> repository)
        {
            _repository = repository;
        }

        public void Handle(CreateClientCommand message)
        {
            var item = new Client(message.Id, message.Name);
            _repository.Save(item, -1);
        }

        public void Handle(RemoveClientCommand message)
        {
            var item = _repository.GetById(message.Id);
            item.Remove();
            _repository.Save(item, message.OriginalVersion);
        }

        public void Handle(RenameClientCommand message)
        {
            var item = _repository.GetById(message.Id);
            item.ChangeName(message.NewName);
            _repository.Save(item, message.OriginalVersion);
        }
    }
}
