using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core;
using CQRS.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ProductService.Controllers
{
    [Route("")]
    public class ProductsController : Controller
    {
        InProcessBus _bus;
        IProductReadModel _readmodel;

        public ProductsController(InProcessBus bus, IProductReadModel readmodel)
        {
            _bus = bus;
            _readmodel = readmodel;
        }

        [HttpGet]
        public IEnumerable<ProductListDto> Get()
        {
            return _readmodel.GetProductListDto();
        }

        [HttpGet("{id}")]
        public ProductDetailDto Get(Guid id)
        {
            return _readmodel.GetProductDetailDto(id);
        }

        [HttpPost]
        public void Post(string name)
        {
            _bus.Send(new CreateProductCommand(Guid.NewGuid(), name));
        }

        [HttpPut]
        public void ChangeName(Guid id, string name, int version)
        {
            var command = new RenameProductCommand(id, name, version);
            _bus.Send(command);
        }

        [HttpDelete]
        public void Delete(Guid id)
        {
            ViewData.Model = _readmodel.GetProductDetailDto(id);
        }
    }
}
