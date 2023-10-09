using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TicketBookingService.Models;
using TicketBookingService.Services;

namespace TicketBookingService.Controllers
{
    [Route("api/trains")]
    [ApiController]
    public class TrainController : ControllerBase
    {
        private readonly ITrainService _trainService;

        public TrainController(ITrainService trainService)
        {
            _trainService = trainService;
        }

        [HttpPost]
        public IActionResult CreateTrain(Train train)
        {
            var createdTrain = _trainService.Create(train);
            return Ok(createdTrain);
        }

        [HttpGet]
        public IActionResult GetAllTrains()
        {
            var allTrains = _trainService.GetAllTrains();
            return Ok(allTrains);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTrain(string id, Train train)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ObjectId format");
            }

            var updatedTrain = _trainService.Update(objectId, train);
            if (updatedTrain == null)
            {
                return NotFound();
            }

            return Ok(updatedTrain);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTrain(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ObjectId format");
            }

            var deletedTrain = _trainService.Delete(objectId);
            if (deletedTrain == null)
            {
                return NotFound();
            }

            return Ok(deletedTrain);
        }
    }
}
