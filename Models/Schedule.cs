using MongoDB.Bson;
namespace TicketBookingService.Models
{
    public class Schedule
    {
        public ObjectId Id { get; set; }
        public ObjectId TrainId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}