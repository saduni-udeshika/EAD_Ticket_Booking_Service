using MongoDB.Driver;
using TicketBookingService.Controllers;
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
            return _trainCollection.Find(train => train.IsActive).ToList();
        }

        public List<Train> GetInactiveTrains()
        {
            return _trainCollection.Find(train => !train.IsActive).ToList();
        }

        public List<Train> GetAllTrains()
        {
            return _trainCollection.Find(_ => true).ToList();
        }

        public Train GetTrainById(string id)
        {
            return _trainCollection.Find(train => train.Id == id).FirstOrDefault();
        }

        public Train Update(string id, Train updatedTrain)
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

        public Train UpdateTrainStatus(string id, UpdateTrainStatusRequest request)
        {
            var filter = Builders<Train>.Filter.Eq(train => train.Id, id);
            var update = Builders<Train>.Update.Set(train => train.IsActive, request.IsActive);

            var options = new FindOneAndUpdateOptions<Train>
            {
                ReturnDocument = ReturnDocument.After
            };

            var updatedTrain = _trainCollection.FindOneAndUpdate(filter, update, options);
            return updatedTrain;
        }

        public Train UpdateArrivalTime(string id, UpdateArrivalTimeRequest request)
        {
            var filter = Builders<Train>.Filter.Eq(train => train.Id, id);
            var update = Builders<Train>.Update.Set(train => train.TrainSchedule.ArrivalTime, request.ArrivalTime);

            var options = new FindOneAndUpdateOptions<Train>
            {
                ReturnDocument = ReturnDocument.After
            };

            var updatedTrain = _trainCollection.FindOneAndUpdate(filter, update, options);
            return updatedTrain;
        }

        public Train UpdateDepartureTime(string id, UpdateDepartureTimeRequest request)
        {
            var filter = Builders<Train>.Filter.Eq(train => train.Id, id);
            var update = Builders<Train>.Update.Set(train => train.TrainSchedule.DepartureTime, request.DepartureTime);

            var options = new FindOneAndUpdateOptions<Train>
            {
                ReturnDocument = ReturnDocument.After
            };

            var updatedTrain = _trainCollection.FindOneAndUpdate(filter, update, options);
            return updatedTrain;
        }

        public Train Delete(string id)
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
        Train GetTrainById(string id);
        Train Update(string id, Train updatedTrain);
        Train Delete(string id);
        Train UpdateTrainStatus(string id, UpdateTrainStatusRequest request);
        Train UpdateArrivalTime(string id, UpdateArrivalTimeRequest request);
        Train UpdateDepartureTime(string id, UpdateDepartureTimeRequest request);
    }
}
