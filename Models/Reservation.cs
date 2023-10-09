using MongoDB.Bson;
namespace TicketBookingService.Models
{
    public class Reservation
    {
        public ObjectId Id { get; set; }
        public string ReferenceId { get; set; }
        public DateTime ReservationDate { get; set; }
        public string? PassengerName { get; set; }
        public string? PhoneNumber { get; set; }
        public string Destination { get; set; } // Added Destination field
        public DateTime Time { get; set; } // Added Time field
        public bool IsCancelled { get; set; }
        
    }

 
}
