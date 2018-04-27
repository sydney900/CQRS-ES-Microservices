using System;
using System.Collections.Generic;

namespace CQRS.Domain
{
    public interface IProductReadModel
    {
        IEnumerable<ProductListDto> GetProductListDto();
        ProductDetailDto GetProductDetailDto(Guid id);
    }
}
