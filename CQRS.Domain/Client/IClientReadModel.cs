using System;
using System.Collections.Generic;

namespace CQRS.Domain
{
    public interface IClientReadModel
    {
        IEnumerable<ClientListDto> GetClientListDto();
        ClientDetailDto GetClientDetailDto(Guid id);
    }
}
