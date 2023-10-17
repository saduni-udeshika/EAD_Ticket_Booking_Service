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
                // Call the service to create the reservation
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

   
        
        [HttpGet("{id}")]
        public IActionResult GetReservation(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ObjectId format");
            }

            var reservation = _reservationService.GetReservationById(objectId);

            if (reservation == null)
            {
                return NotFound("Reservation not found");
            }

            return Ok(reservation);
        }

     

     [HttpPut("{id}")]
        public IActionResult UpdateReservation(string id, Reservation reservation)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ObjectId format");
            }

            try
            {
                // Call the service to update the reservation
                var updatedReservation = _reservationService.Update(objectId, reservation);

                if (updatedReservation == null)
                {
                    return NotFound("Reservation not found");
                }

                return Ok(updatedReservation);
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

        
           [HttpDelete("{id}")]
        public IActionResult DeleteReservation(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ObjectId format");
            }

            try
            {
                // Call the service to cancel the reservation
                var canceledReservation = _reservationService.Cancel(objectId);

                if (canceledReservation == null)
                {
                    return NotFound("Reservation not found");
                }

                return Ok(canceledReservation);
            }
            catch (ArgumentException ex)
            {
                // Handle reservation date validation error
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
