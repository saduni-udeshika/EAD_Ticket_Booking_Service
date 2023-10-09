using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TicketBookingService.Models;
using TicketBookingService.Services;
using System;
using System.Collections.Generic;

namespace TicketBookingService.Controllers
{
    [Route("api/travelers")]
    [ApiController]
    public class TravelerController : ControllerBase
    {
        private readonly ITravelerService _travelerService;

        public TravelerController(ITravelerService travelerService)
        {
            _travelerService = travelerService;
        }

        [HttpPost]
        public IActionResult CreateTraveler(Traveler traveler)
        {
            var createdTraveler = _travelerService.Create(traveler);
            return Ok(createdTraveler);
        }

        [HttpGet]
        public IActionResult GetAllTravelers()
        {
            var allTravelers = _travelerService.GetAllTravelers();
            return Ok(allTravelers);
        }

        [HttpGet("{id}")]
        public IActionResult GetTravelerById(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ObjectId format");
            }

            var traveler = _travelerService.GetTravelerById(objectId);
            if (traveler == null)
            {
                return NotFound();
            }

            return Ok(traveler);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTraveler(string id, Traveler traveler)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ObjectId format");
            }

            var updatedTraveler = _travelerService.Update(objectId, traveler);
            if (updatedTraveler == null)
            {
                return NotFound();
            }

            return Ok(updatedTraveler);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTraveler(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ObjectId format");
            }

            var deletedTraveler = _travelerService.Delete(objectId);
            if (deletedTraveler == null)
            {
                return NotFound();
            }

            return Ok(deletedTraveler);
        }
    }
}
