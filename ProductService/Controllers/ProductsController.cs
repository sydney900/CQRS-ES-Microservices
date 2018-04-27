using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        InProcessBus _bus;
        IProductReadModel _readmodel;

        public ProductsController(InProcessBus bus, IProductReadModel readmodel)
        {
            _bus = bus;
            _readmodel = readmodel;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<ProductListDto> Get()
        {
            return _readmodel.GetProductListDto();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ProductDetailDto Get(Guid id)
        {
            return _readmodel.GetProductDetailDto(id);
        }

        // POST api/values
        [HttpPost]
        public void Post(string name)
        {
            _bus.Send(new CreateProductCommand(Guid.NewGuid(), name));
        }

        [HttpPost]
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
