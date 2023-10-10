using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TicketBookingService.Models;
using TicketBookingService.Services;

namespace TicketBookingService.Controllers
{
    [Route("api/reservation")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly ITrainService _trainService;

        public ReservationController(IReservationService reservationService, ITrainService trainService)
        {
            _reservationService = reservationService;
            _trainService = trainService;
        }

        [HttpPost]
        public IActionResult CreateReservation(Reservation reservation)
        {
            var createdReservation = _reservationService.Create(reservation);
            // Assuming you want to return the updated train information
            var updatedTrain = _trainService.GetTrainById(new ObjectId(reservation.TrainId));

            return Ok(updatedTrain);
        }

        [HttpGet]
        public IActionResult GetAllReservations()
        {
            var allReservations = _reservationService.GetAllReservations();
            return Ok(allReservations);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReservation(string id, Reservation reservation)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ObjectId format");
            }

            var updatedReservation = _reservationService.Update(objectId, reservation);
            if (updatedReservation == null)
            {
                return NotFound();
            }

            return Ok(updatedReservation);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReservation(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ObjectId format");
            }

            var deletedReservation = _reservationService.Delete(objectId);
            if (deletedReservation == null)
            {
                return NotFound();
            }

            return Ok(deletedReservation);
        }
    }
}
