using System;
using System.Collections.Generic;

namespace CQRS.Domain
{
    public class ClientReadModel : IClientReadModel
    {
        public IEnumerable<ClientListDto> GetClientListDto()
        {
            return ClientMemoryDatabase.list;
        }

        public ClientDetailDto GetClientDetailDto(Guid id)
        {
            ClientDetailDto client = null;
            ClientMemoryDatabase.details.TryGetValue(id, out client);

            return client;
        }
    }
}
