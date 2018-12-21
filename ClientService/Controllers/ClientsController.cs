using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core;
using CQRS.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiCommon.Filters;

namespace ClientService.Controllers
{
    [Authorize]
    [Route("")]
    public class ClientsController : Controller
    {
        InProcessBus _bus;
        IClientReadModel _readmodel;

        public ClientsController(InProcessBus bus, IClientReadModel readmodel)
        {
            _bus = bus;
            _readmodel = readmodel;
        }

        /// <summary>
        /// Return all your clients
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ClientListDto>), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public IEnumerable<ClientListDto> Get()
        {
            return _readmodel.GetClientListDto();
        }

        /// <summary>
        /// Get a client detail for a given client ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClientDetailDto), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ClientDetailDto Get(Guid id)
        {
            return _readmodel.GetClientDetailDto(id);
        }

        /// <summary>
        /// Create a new client
        /// </summary>
        /// <param name="name"></param>
        [HttpPost]
        [ServiceFilter(typeof(ValidateModelAttribute))]
        public void Post([FromBody]ClientDetailDto client)
        {
            _bus.Send(new CreateClientCommand(Guid.NewGuid(), client.Name));
        }

        /// <summary>
        /// Change a client name
        /// </summary>
        /// <param name="id"></param>
        /// <param name="client"></param>
        [HttpPut("ChangeName/{id:guid}")]
        [ServiceFilter(typeof(ValidateModelAttribute))]
        public void ChangeName(Guid id, [FromBody]ClientDetailDto client)
        {
            var command = new RenameClientCommand(id, client.Name, client.Version);
            _bus.Send(command);
        }

        /// <summary>
        /// Delete a client
        /// </summary>
        /// <param name="id"></param>
        [Route("{id:guid}")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            ClientDetailDto client = _readmodel.GetClientDetailDto(id);

            if (client != null )
            {
                var command = new RemoveClientCommand(id, client.Version);

                _bus.Send(command);
            }
        }
    }
}
