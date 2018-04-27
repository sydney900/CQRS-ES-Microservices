using System;
using System.Collections.Generic;

namespace CQRS.Domain
{
    public class ProductReadModel : IProductReadModel
    {
        public IEnumerable<ProductListDto> GetProductListDto()
        {
            return ProductMemoryDatabase.list;
        }

        public ProductDetailDto GetProductDetailDto(Guid id)
        {
            return ProductMemoryDatabase.details[id];
        }
    }
}
