using MongoDB.Bson;
using MongoDB.Driver;
using TicketBookingService.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace TicketBookingService.Services
{
    public class BackofficeService : IBackofficeService
    {
        private readonly IMongoCollection<Backoffice> _backofficeCollection;

        public BackofficeService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("TicketBookingApp"));
            var database = client.GetDatabase("TrainBookingDB");
            _backofficeCollection = database.GetCollection<Backoffice>("backoffices");
        }

        public Backoffice Create(Backoffice backoffice)
        {
            _backofficeCollection.InsertOne(backoffice);
            return backoffice;
        }

        public List<Backoffice> GetAllBackoffices()
        {
            return _backofficeCollection.Find(_ => true).ToList();
        }

        public Backoffice GetBackofficeById(ObjectId id)
        {
            return _backofficeCollection.Find(backoffice => backoffice.Id == id).FirstOrDefault();
        }

        public Backoffice Update(ObjectId id, Backoffice updatedBackoffice)
        {
            var filter = Builders<Backoffice>.Filter.Eq(backoffice => backoffice.Id, id);
            var update = Builders<Backoffice>.Update
                .Set(backoffice => backoffice.Username, updatedBackoffice.Username)
                .Set(backoffice => backoffice.Password, updatedBackoffice.Password)
                .Set(backoffice => backoffice.Nic, updatedBackoffice.Nic)
                .Set(backoffice => backoffice.IsActive, updatedBackoffice.IsActive);

            var options = new FindOneAndUpdateOptions<Backoffice>
            {
                ReturnDocument = ReturnDocument.After
            };

            return _backofficeCollection.FindOneAndUpdate(filter, update, options);
        }

        public Backoffice Delete(ObjectId id)
        {
            var deletedBackoffice = _backofficeCollection.FindOneAndDelete(backoffice => backoffice.Id == id);
            return deletedBackoffice;
        }
    }

    public interface IBackofficeService
    {
        Backoffice Create(Backoffice backoffice);
        List<Backoffice> GetAllBackoffices();
        Backoffice GetBackofficeById(ObjectId id);
        Backoffice Update(ObjectId id, Backoffice updatedBackoffice);
        Backoffice Delete(ObjectId id);
    }
}
