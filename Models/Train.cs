
using MongoDB.Bson;
namespace TicketBookingService.Models
{
    public class Train
    {
        public ObjectId Id { get; set; }
        public string? TrainName { get; set; }
        public string? TrainNumber { get; set; }
        public string? Destination { get; set; }
        public bool IsActive { get; set; }
        public Schedule? TrainSchedule { get; set; }
    }

        public class Schedule
    {
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }

}  