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

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost]
        public IActionResult CreateReservation(Reservation reservation)
        {
            var createdReservation = _reservationService.Create(reservation);
            return Ok(createdReservation);
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
