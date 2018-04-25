using System;
using System.Collections.Generic;

namespace CQRS.Domain
{
    public static class ClientMemoryDatabase
    {
        public static Dictionary<Guid, ClientDetailDto> details = new Dictionary<Guid, ClientDetailDto>();
        public static List<ClientListDto> list = new List<ClientListDto>();
    }
}
