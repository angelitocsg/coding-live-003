using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyApiWithDoc.Entities;
using MyApiWithDoc.MockData;
using MyApiWithDoc.Requests;
using MyApiWithDoc.Responses;
using static Bogus.DataSets.Name;

namespace MyApiWithDoc.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly ILogger<ClientsController> _logger;
        private static ClientMock mockClients;

        public ClientsController(ILogger<ClientsController> logger)
        {
            _logger = logger;
            if (mockClients == null)
            {
                _logger.LogInformation("Creating mock data");
                mockClients = new ClientMock();
            }
        }

        /// <summary>
        /// Get the client response by client id
        /// </summary>
        /// <param name="id">Client id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ClientResponse> GetById([FromRoute] int id)
        {
            if (id <= 0)
                return BadRequest();

            var clientResponse = FilterById(id);
            if (clientResponse == null)
                return NotFound();

            return Ok(clientResponse);
        }

        /// <summary>
        /// Get the list of clients by name and/or gender
        /// </summary>
        /// <param name="name">Part of name</param>
        /// <param name="gender">Male or Female</param>
        /// <returns></returns>
        [HttpGet("search")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<ClientResponse>> GetByFilter(
            [FromQuery] string name = null,
            [FromQuery] Gender? gender = null)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!string.IsNullOrEmpty(name) && gender == null)
            {
                var clientsResponse = FilterByName(name);
                if (clientsResponse == null)
                    NotFound();

                return Ok(clientsResponse);
            }

            else if (string.IsNullOrEmpty(name) && gender != null)
            {
                var clientsResponse = FilterByGender(gender);
                if (clientsResponse == null)
                    NotFound();

                return Ok(clientsResponse);
            }

            else if (!string.IsNullOrEmpty(name) && gender != null)
            {
                var clientsResponse = FilterByNameAndGender(name, gender);
                if (clientsResponse == null)
                    NotFound();

                return Ok(clientsResponse);
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// Get the list of all clients
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<ClientResponse>> GetAll()
        {
            var clientsResponse = NoFilter();
            if (clientsResponse == null)
                return NoContent();

            return Ok(clientsResponse);
        }

        /// <summary>
        /// Add a new client
        /// </summary>
        /// <param name="clientRequest">Client data</param>
        /// <returns></returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<int> Post(
            [FromBody] CreateClientRequest clientRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var nextId = mockClients.Data.Max(c => c.Id) + 1;
            var client = new Client(
                id: nextId,
                name: clientRequest.Name,
                email: clientRequest.Email,
                gender: clientRequest.Gender,
                phone: clientRequest.Phone);

            mockClients.Add(client);

            return CreatedAtAction(nameof(Post), new { Id = client.Id });
        }

        /// <summary>
        /// Update an existing clients
        /// </summary>
        /// <param name="id">Client id</param>
        /// <param name="clientRequest">Client data</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<int> Put(
            [FromRoute] int id,
            [FromBody] UpdateClientRequest clientRequest)
        {
            if (!ModelState.IsValid || id != clientRequest.Id)
                return BadRequest();

            var client = mockClients.GetById(id);
            if (client == null)
                return NotFound();

            client.Name = clientRequest.Name;
            client.Email = clientRequest.Email;
            client.Gender = clientRequest.Gender;
            client.Phone = clientRequest.Phone;

            mockClients.Update(client);

            return Ok(new { Id = client.Id });
        }

        /// <summary>
        /// Enable or disable a client by client id
        /// </summary>
        /// <param name="id">Client id</param>
        /// <param name="enabled">true or false</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<int> Patch(
            [FromRoute] int id,
            [FromQuery] bool enabled)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var client = mockClients.GetById(id);
            if (client == null)
                return NotFound();

            client.Enabled = enabled;

            mockClients.Update(client);

            return Ok(new { Id = client.Id });
        }

        /// <summary>
        /// Remove a client by client id
        /// </summary>
        /// <param name="id">Client id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var client = mockClients.GetById(id);
            if (client == null)
                return NotFound();

            mockClients.Remove(id);

            return NoContent();
        }

        private IEnumerable<ClientResponse> NoFilter()
        {
            return from client in mockClients.Data
                   select new ClientResponse(
                       id: client.Id,
                       name: client.Name,
                       email: client.Email,
                       gender: client.Gender,
                       phone: client.Phone,
                       enabled: client.Enabled
                   );
        }
        private ClientResponse FilterById(int id)
        {
            return (from client in mockClients.Data
                    where client.Id.Equals(id)
                    select new ClientResponse(
                        id: client.Id,
                        name: client.Name,
                        email: client.Email,
                        gender: client.Gender,
                        phone: client.Phone,
                        enabled: client.Enabled
                    )).FirstOrDefault();
        }
        private IEnumerable<ClientResponse> FilterByName(string name)
        {
            return from client in mockClients.Data
                   where client.Name.Contains(name)
                   select new ClientResponse(
                       id: client.Id,
                       name: client.Name,
                       email: client.Email,
                       gender: client.Gender,
                       phone: client.Phone,
                       enabled: client.Enabled
                   );
        }

        private IEnumerable<ClientResponse> FilterByGender(Gender? gender)
        {
            return from client in mockClients.Data
                   where client.Gender.Equals(gender)
                   select new ClientResponse(
                       id: client.Id,
                       name: client.Name,
                       email: client.Email,
                       gender: client.Gender,
                       phone: client.Phone,
                       enabled: client.Enabled
                   );
        }

        private object FilterByNameAndGender(string name, Gender? gender)
        {
            return from client in mockClients.Data
                   where client.Name.Contains(name) && client.Gender.Equals(gender)
                   select new ClientResponse(
                       id: client.Id,
                       name: client.Name,
                       email: client.Email,
                       gender: client.Gender,
                       phone: client.Phone,
                       enabled: client.Enabled
                   );
        }
    }
}
