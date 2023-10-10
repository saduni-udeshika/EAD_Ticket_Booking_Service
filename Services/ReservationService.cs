using MongoDB.Bson;
using MongoDB.Driver;
using TicketBookingService.Models;

namespace TicketBookingService.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IMongoCollection<Reservation> _reservationCollection;
        private readonly IMongoCollection<Train> _trainCollection;

        public ReservationService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("TicketBookingApp"));
            var database = client.GetDatabase("TicketBookingDB");
            _reservationCollection = database.GetCollection<Reservation>("reservations");
            _trainCollection = database.GetCollection<Train>("train");
        }

        public Reservation Create(Reservation reservation)
        {
            _reservationCollection.InsertOne(reservation);
            // After creating a reservation, update the associated train's IsActive and IsPublished
            var associatedTrain = _trainCollection.Find(train => train.Id.ToString() == reservation.TrainId).FirstOrDefault();
            if (associatedTrain != null)
            {
                associatedTrain.IsActive = true;
                associatedTrain.IsPublished = true;
                _trainCollection.ReplaceOne(train => train.Id == associatedTrain.Id, associatedTrain);
            }
            return reservation;
        }

        public List<Reservation> GetAllReservations()
        {
            return _reservationCollection.Find(_ => true).ToList();
        }

        public Reservation GetReservationById(ObjectId id)
        {
            return _reservationCollection.Find(reservation => reservation.Id == id).FirstOrDefault();
        }

        public Reservation Update(ObjectId id, Reservation updatedReservation)
        {
            var filter = Builders<Reservation>.Filter.Eq(reservation => reservation.Id, id);
            var update = Builders<Reservation>.Update

                .Set(reservation => reservation.PhoneNumber, updatedReservation.PhoneNumber)
                .Set(reservation => reservation.ReservationDate, updatedReservation.ReservationDate)
                .Set(reservation => reservation.Destination, updatedReservation.Destination)
                .Set(reservation => reservation.Time, updatedReservation.Time);

            var options = new FindOneAndUpdateOptions<Reservation>
            {
                ReturnDocument = ReturnDocument.After
            };

            return _reservationCollection.FindOneAndUpdate(filter, update, options);
        }

        public Reservation Delete(ObjectId id)
        {
            var deletedReservation = _reservationCollection.FindOneAndDelete(reservation => reservation.Id == id);
            return deletedReservation;
        }

        public bool HasExistingReservationsForTrain(string trainId)
        {
            var existingReservations = _reservationCollection
                .Find(reservation => reservation.TrainId == trainId)
                .ToList();

            return existingReservations.Count > 0;
        }
    }

    public interface IReservationService
    {
        Reservation Create(Reservation reservation);
        List<Reservation> GetAllReservations();
        Reservation GetReservationById(ObjectId id);
        Reservation Update(ObjectId id, Reservation updatedReservation);
        Reservation Delete(ObjectId id);

        bool HasExistingReservationsForTrain(string trainId);
    }
}
