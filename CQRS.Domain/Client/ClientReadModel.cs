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
            return ClientMemoryDatabase.details[id];
        }
    }
}
