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

        [HttpDelete("cancel/{id}")]
        public IActionResult CancelTrain(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId trainObjectId))
            {
                return BadRequest("Invalid train ObjectId format");
            }

            // Check if there are any existing reservations for this train
            // TODO: Uncomment line 59 and remove line 61 (hardcoded value)
            // bool hasExistingReservations = _reservationService.HasExistingReservationsForTrain(trainObjectId);
            bool hasExistingReservations = false;

            if (hasExistingReservations)
            {
                return BadRequest("Cannot cancel a train with existing reservations.");
            }

            // Train cancellation
            var canceledTrain = _trainService.Delete(trainObjectId);
            if (canceledTrain == null)
            {
                return NotFound();
            }

            return Ok("Train successfully canceled.");
        }

    }
}
