using MongoDB.Bson;
namespace TicketBookingService.Models
{
    public class Reservation
    {
        public ObjectId Id { get; set; }
        // Add a property to represent the association with a train
        public string ReferenceId { get; set; }
        public ObjectId TrainId { get; set; }
        public DateTime ReservationDate { get; set; }
        public string? PassengerName { get; set; }
        public string? PhoneNumber { get; set; }
        public string Destination { get; set; }
        public DateTime Time { get; set; }
        public bool IsCancelled { get; set; }
        
    }

 
}
