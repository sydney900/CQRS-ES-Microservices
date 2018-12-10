using System;

namespace CQRS.Domain
{
    public class ClientDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }

        public ClientDetailDto(Guid id, string name, int version = 1)
        {
            Id = id;
            Name = name;
            Version = version;
        }
    }
}
