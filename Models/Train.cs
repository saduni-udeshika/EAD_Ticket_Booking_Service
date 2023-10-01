using MongoDB.Bson;
namespace TicketBookingService.Models
{
    public class Train
    {
        public ObjectId Id { get; set; }
        public string TrainName { get; set; }
        public string TrainNumber { get; set; }
    }

}
