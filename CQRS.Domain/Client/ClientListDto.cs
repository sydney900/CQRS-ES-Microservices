using System;

namespace CQRS.Domain
{
    public class ClientListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ClientListDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
