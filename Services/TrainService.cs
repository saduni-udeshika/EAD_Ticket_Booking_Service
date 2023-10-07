using MongoDB.Driver;
using TicketBookingService.Models;

namespace TicketBookingService.Services
{
    public class TrainService : ITrainService
    {
        private readonly IMongoCollection<Train> _trainCollection;

        public TrainService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("TicketBookingApp"));
            var database = client.GetDatabase("TrainBookingDB");
            _trainCollection = database.GetCollection<Train>("trains");
        }

        public Train Create(Train train)
        {
            _trainCollection.InsertOne(train);
            return train;
        }
    }

    public interface ITrainService
    {
        Train Create(Train train);
    }
}
