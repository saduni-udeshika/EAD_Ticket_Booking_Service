using System;
using MongoDB.Bson.Serialization.Attributes;

namespace TicketBookingService.Models
{
    public class Train
    {
        [BsonId]
        public required string Id { get; set; }
        public required string TrainName { get; set; }
        public required string TrainNumber { get; set; }
        public required string Destination { get; set; }
        public bool IsActive { get; set; }
        public required Schedule TrainSchedule { get; set; }
    }

    public class Schedule
    {
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
