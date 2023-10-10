using MongoDB.Bson;
using MongoDB.Driver;
using TicketBookingService.Models;
using Microsoft.Extensions.Configuration; // Make sure you have this using statement

namespace TicketBookingService.Services
{
    public class TravelagentService : ITravelagentService
    {
        private readonly IMongoCollection<Travelagent> _travelagentCollection;

        public TravelagentService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("TicketBookingApp"));
            var database = client.GetDatabase("TrainBookingDB");
            _travelagentCollection = database.GetCollection<Travelagent>("travelagents");
        }

        public Travelagent Create(Travelagent travelagent)
        {
            _travelagentCollection.InsertOne(travelagent);
            return travelagent;
        }

        public List<Travelagent> GetAllTravelagents()
        {
            return _travelagentCollection.Find(_ => true).ToList();
        }

        public Travelagent GetTravelagentById(ObjectId id)
        {
            return _travelagentCollection.Find(travelagent => travelagent.Id == id).FirstOrDefault();
        }

        public Travelagent Update(ObjectId id, Travelagent updatedTravelagent)
        {
            var filter = Builders<Travelagent>.Filter.Eq(travelagent => travelagent.Id, id);
            var update = Builders<Travelagent>.Update
                .Set(travelagent => travelagent.Name, updatedTravelagent.Name)
                .Set(travelagent => travelagent.Email, updatedTravelagent.Email)
                .Set(travelagent => travelagent.Password, updatedTravelagent.Password)
                .Set(travelagent => travelagent.Nic, updatedTravelagent.Nic)
                .Set(travelagent => travelagent.IsActive, updatedTravelagent.IsActive);

            var options = new FindOneAndUpdateOptions<Travelagent>
            {
                ReturnDocument = ReturnDocument.After
            };

            return _travelagentCollection.FindOneAndUpdate(filter, update, options);
        }

        public Travelagent Delete(ObjectId id)
        {
            var deletedTravelagent = _travelagentCollection.FindOneAndDelete(travelagent => travelagent.Id == id);
            return deletedTravelagent;
        }
    }

    public interface ITravelagentService
    {
        Travelagent Create(Travelagent travelagent);
        List<Travelagent> GetAllTravelagents();
        Travelagent GetTravelagentById(ObjectId id);
        Travelagent Update(ObjectId id, Travelagent updatedTravelagent);
        Travelagent Delete(ObjectId id);
    }
}
