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
        private readonly IReservationService _reservationService;

        public TrainController(ITrainService trainService, IReservationService reservationService)
        {
            _trainService = trainService;
            _reservationService = reservationService;
        }

        [HttpPost]
        public IActionResult CreateTrain(Train train)
        {
            var createdTrain = _trainService.Create(train);
            return Ok(createdTrain);
        }

        [HttpGet]
        public IActionResult GetAllTrains([FromQuery] bool isActive = true)
        {
            List<Train> trains;

            if (isActive)
            {
                // Retrieve active trains
                trains = _trainService.GetActiveTrains();
            }
            else
            {
                // Retrieve inactive trains
                trains = _trainService.GetInactiveTrains();
            }

            return Ok(trains);
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
        public IActionResult CancelTrain(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId trainObjectId))
            {
                return BadRequest("Invalid train ObjectId format");
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