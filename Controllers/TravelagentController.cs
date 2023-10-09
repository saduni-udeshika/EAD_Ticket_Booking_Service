using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TicketBookingService.Models;
using TicketBookingService.Services;
using System;
using System.Collections.Generic;

namespace TicketBookingService.Controllers
{
    [Route("api/travelagents")]
    [ApiController]
    public class TravelagentController : ControllerBase
    {
        private readonly ITravelagentService _travelagentService;

        public TravelagentController(ITravelagentService travelagentService)
        {
            _travelagentService = travelagentService;
        }

        [HttpPost]
        public IActionResult CreateTraveleragent(Travelagent travelagent)
        {
            var createdTravelagent = _travelagentService.Create(travelagent);
            return Ok(createdTravelagent);
        }

        [HttpGet]
        public IActionResult GetAllTravelagents()
        {
            var allTravelagents = _travelagentService.GetAllTravelagents();
            return Ok(allTravelagents);
        }

        [HttpGet("{id}")]
        public IActionResult GetTravelagentById(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ObjectId format");
            }

            var travelagent = _travelagentService.GetTravelagentById(objectId);
            if (travelagent == null)
            {
                return NotFound();
            }

            return Ok(travelagent);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTravelagent(string id, Travelagent travelagent)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ObjectId format");
            }

            var updatedTravelagent = _travelagentService.Update(objectId, travelagent);
            if (updatedTravelagent == null)
            {
                return NotFound();
            }

            return Ok(updatedTravelagent);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTravelagent(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ObjectId format");
            }

            var deletedTravelagent = _travelagentService.Delete(objectId);
            if (deletedTravelagent == null)
            {
                return NotFound();
            }

            return Ok(deletedTravelagent);
        }
    }
}
