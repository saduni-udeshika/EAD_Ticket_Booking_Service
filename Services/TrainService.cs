using MongoDB.Bson;
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
            var database = client.GetDatabase("TicketBookingDB");
            _trainCollection = database.GetCollection<Train>("trains");
        }

        public Train Create(Train train)
        {
            _trainCollection.InsertOne(train);
            return train;
        }

        public List<Train> GetActiveTrains()
        {
            return _trainCollection.Find(train => train.IsActive == true).ToList();
        }

        public List<Train> GetInactiveTrains()
        {
            return _trainCollection.Find(train => train.IsActive == false).ToList();
        }

        public List<Train> GetAllTrains()
        {
            return _trainCollection.Find(Builders<Train>.Filter.Empty).ToList();
        }

        public Train GetTrainById(ObjectId id)
        {
            return _trainCollection.Find(train => train.Id == id).FirstOrDefault();
        }

        public Train Update(ObjectId id, Train updatedTrain)
        {
            var filter = Builders<Train>.Filter.Eq(train => train.Id, id);
            var update = Builders<Train>.Update
                .Set(train => train.TrainName, updatedTrain.TrainName)
                .Set(train => train.TrainNumber, updatedTrain.TrainNumber)
                .Set(train => train.IsActive, updatedTrain.IsActive);

            var options = new FindOneAndUpdateOptions<Train>
            {
                ReturnDocument = ReturnDocument.After
            };

            return _trainCollection.FindOneAndUpdate(filter, update, options);
        }

        public Train Delete(ObjectId id)
        {
            var deletedTrain = _trainCollection.FindOneAndDelete(train => train.Id == id);
            return deletedTrain;
        }
    }

    public interface ITrainService
    {
        Train Create(Train train);
        List<Train> GetActiveTrains();
        List<Train> GetInactiveTrains();
        List<Train> GetAllTrains();
        Train GetTrainById(ObjectId id);
        Train Update(ObjectId id, Train updatedTrain);
        Train Delete(ObjectId id);
    }
}