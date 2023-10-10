using MongoDB.Bson;
using MongoDB.Driver;
using TicketBookingService.Models;
using Microsoft.Extensions.Configuration; // Make sure you have this using statement

namespace TicketBookingService.Services
{
    public class TravelerService : ITravelerService
    {
        private readonly IMongoCollection<Traveler> _travelerCollection;

        public TravelerService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("TicketBookingApp"));
            var database = client.GetDatabase("TrainBookingDB");
            _travelerCollection = database.GetCollection<Traveler>("travelers");
        }

        public Traveler Create(Traveler traveler)
        {
            _travelerCollection.InsertOne(traveler);
            return traveler;
        }

        public List<Traveler> GetAllTravelers()
        {
            return _travelerCollection.Find(_ => true).ToList();
        }

        public Traveler GetTravelerById(ObjectId id)
        {
            return _travelerCollection.Find(traveler => traveler.Id == id).FirstOrDefault();
        }

        public Traveler Update(ObjectId id, Traveler updatedTraveler)
        {
            var filter = Builders<Traveler>.Filter.Eq(traveler => traveler.Id, id);
            var update = Builders<Traveler>.Update
                .Set(traveler => traveler.Name, updatedTraveler.Name)
                .Set(traveler => traveler.Email, updatedTraveler.Email)
                .Set(traveler => traveler.MobileNum, updatedTraveler.MobileNum)
                .Set(traveler => traveler.Password, updatedTraveler.Password)
                .Set(traveler => traveler.Nic, updatedTraveler.Nic)
                .Set(traveler => traveler.IsActive, updatedTraveler.IsActive);

            var options = new FindOneAndUpdateOptions<Traveler>
            {
                ReturnDocument = ReturnDocument.After
            };

            return _travelerCollection.FindOneAndUpdate(filter, update, options);
        }

        public Traveler Delete(ObjectId id)
        {
            var deletedTraveler = _travelerCollection.FindOneAndDelete(traveler => traveler.Id == id);
            return deletedTraveler;
        }
    }

    public interface ITravelerService
    {
        Traveler Create(Traveler traveler);
        List<Traveler> GetAllTravelers();
        Traveler GetTravelerById(ObjectId id);
        Traveler Update(ObjectId id, Traveler updatedTraveler);
        Traveler Delete(ObjectId id);
    }
}
