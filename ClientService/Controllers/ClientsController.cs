using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ClientService.Controllers
{
    [Route("api/[controller]")]
    public class ClientsController : Controller
    {
        InProcessBus _bus;
        IClientReadModel _readmodel;

        public ClientsController(InProcessBus bus, IClientReadModel readmodel)
        {
            _bus = bus;
            _readmodel = readmodel;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<ClientListDto> Get()
        {
            return _readmodel.GetClientListDto();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ClientDetailDto Get(Guid id)
        {
            return _readmodel.GetClientDetailDto(id);
        }

        // POST api/values
        [HttpPost]
        public void Post(string name)
        {
            _bus.Send(new CreateClientCommand(Guid.NewGuid(), name));
        }

        [HttpPost]
        public void ChangeName(Guid id, string name, int version)
        {
            var command = new RenameClientCommand(id, name, version);
            _bus.Send(command);
        }

        [HttpDelete]
        public void Delete(Guid id)
        {
            ViewData.Model = _readmodel.GetClientDetailDto(id);
        }
    }
}
