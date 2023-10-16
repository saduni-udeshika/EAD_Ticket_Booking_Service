using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
            train.Id = Guid.NewGuid().ToString("N");
            var createdTrain = _trainService.Create(train);
            return Ok(createdTrain);
        }

        [HttpGet]
        public IActionResult GetAllTrains([FromQuery] bool? isActive)
        {
            List<Train> trains = isActive.HasValue
                ? (isActive.Value ? _trainService.GetActiveTrains() : _trainService.GetInactiveTrains())
                : _trainService.GetAllTrains();

            return Ok(trains);
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateTrain(string id, Train train)
        {
            var updatedTrain = _trainService.Update(id, train);
            if (updatedTrain == null)
            {
                return NotFound();
            }

            return Ok(updatedTrain);
        }

        [HttpPut("{id}/status")]
        public IActionResult UpdateTrainStatus(string id, [FromBody] UpdateTrainStatusRequest request)
        {
            if (!Guid.TryParse(id, out Guid guid))
            {
                return BadRequest("Invalid Guid format");
            }

            var trainId = id.ToString();
            var updatedTrain = _trainService.UpdateTrainStatus(trainId, request);
            if (updatedTrain == null)
            {
                return NotFound();
            }

            return Ok(updatedTrain);
        }

        [HttpDelete("{id}")]
        public IActionResult CancelTrain(string id)
        {
            var canceledTrain = _trainService.Delete(id);
            if (canceledTrain == null)
            {
                return NotFound();
            }

            return Ok("Train successfully canceled.");
        }
    }

    public class UpdateTrainStatusRequest
    {
        public bool IsActive { get; set; }
    }

}
