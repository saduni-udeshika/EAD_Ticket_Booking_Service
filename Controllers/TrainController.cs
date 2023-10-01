using Microsoft.AspNetCore.Mvc;
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

        public interface ITrainService
        {
            Train Create(Train train);
        }

    }
}
