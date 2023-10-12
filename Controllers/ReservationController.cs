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
            try
            {
                // Validate and create the reservation
                var createdReservation = _reservationService.Create(reservation);

                return Ok(createdReservation);
            }
            catch (ArgumentException ex)
            {
                // Handle reservation date validation error
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                // Handle maximum reservation limit validation error
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(500, "An error occurred while processing your request.");
            }
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
            var existingReservation = _reservationService.GetReservationById(objectId);
            if (existingReservation == null)
            {
                return BadRequest("existingReservation");
            }
            // Calculate the minimum allowed reservation date (5 days from now).
            var minAllowedReservationDate = DateTime.UtcNow.AddDays(5);
            if (reservation.ReservationDate < minAllowedReservationDate)
            {
                return BadRequest("Reservation date must be at least 5 days in the future.");
            }
            var updatedReservation = _reservationService.Update(objectId, reservation);
            if (updatedReservation == null)
            {
                return BadRequest("updateReservation");
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
