using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TicketBookingService.Models;
using TicketBookingService.Services;
using System;
using System.Collections.Generic;

namespace TicketBookingService.Controllers
{
    [Route("api/backoffices")] // Change the route to "backoffices"
    [ApiController]
    public class BackofficeController : ControllerBase // Change the class name to "BackofficeController"
    {
        private readonly IBackofficeService _backofficeService;

        public BackofficeController(IBackofficeService backofficeService)
        {
            _backofficeService = backofficeService;
        }

        [HttpPost]
        public IActionResult CreateBackoffice(Backoffice backoffice) // Change the parameter type to "Backoffice"
        {
            var createdBackoffice = _backofficeService.Create(backoffice);
            return Ok(createdBackoffice);
        }

        [HttpGet]
        public IActionResult GetAllBackoffices() // Change the method name to "GetAllBackoffices"
        {
            var allBackoffices = _backofficeService.GetAllBackoffices();
            return Ok(allBackoffices);
        }

        [HttpGet("{id}")]
        public IActionResult GetBackofficeById(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ObjectId format");
            }

            var backoffice = _backofficeService.GetBackofficeById(objectId);
            if (backoffice == null)
            {
                return NotFound();
            }

            return Ok(backoffice);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBackoffice(string id, Backoffice backoffice) // Change the parameter type to "Backoffice"
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ObjectId format");
            }

            var updatedBackoffice = _backofficeService.Update(objectId, backoffice);
            if (updatedBackoffice == null)
            {
                return NotFound();
            }

            return Ok(updatedBackoffice);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBackoffice(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ObjectId format");
            }

            var deletedBackoffice = _backofficeService.Delete(objectId);
            if (deletedBackoffice == null)
            {
                return NotFound();
            }

            return Ok(deletedBackoffice);
        }
    }
}
