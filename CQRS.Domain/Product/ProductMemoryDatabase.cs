using System;
using System.Collections.Generic;

namespace CQRS.Domain
{
    public static class ProductMemoryDatabase
    {
        public static Dictionary<Guid, ProductDetailDto> details = new Dictionary<Guid, ProductDetailDto>();
        public static List<ProductListDto> list = new List<ProductListDto>();
    }
}
