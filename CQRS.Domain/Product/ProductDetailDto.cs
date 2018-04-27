using System;

namespace CQRS.Domain
{
    public class ProductDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }

        public ProductDetailDto(Guid id, string name, int version)
        {
            Id = id;
            Name = name;
            Version = version;
        }
    }
}
