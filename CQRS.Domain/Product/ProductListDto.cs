using System;

namespace CQRS.Domain
{
    public class ProductListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ProductListDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
