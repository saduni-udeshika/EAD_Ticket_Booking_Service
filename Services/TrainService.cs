using System;
using System.Collections.Generic;
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

        public Train UpdateTrainStatus(string id, Train updateisActive)
        {
            var filter = Builders<Train>.Filter.Eq(train => train.Id, id);
            var update = Builders<Train>.Update.Set(train => train.IsActive, updateisActive.IsActive);

            var options = new FindOneAndUpdateOptions<Train>
            {
                ReturnDocument = ReturnDocument.After
            };

            return _trainCollection.FindOneAndUpdate(filter, update, options);
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
        Train UpdateTrainStatus(string id, Train updateisActive);
        Train Delete(string id);
    }
}
