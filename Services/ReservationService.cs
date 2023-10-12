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
            // Check if the reservation date is within 30 days from the booking date
            if ((reservation.ReservationDate - DateTime.Now).TotalDays > 30)
            {
                throw new ArgumentException("Reservation date must be within 30 days from the booking date.");
            }

            // Check if there are already 4 reservations with the same reference ID
            var existingReservations = _reservationCollection.Find(r => r.ReferenceId == reservation.ReferenceId).ToList();
            if (existingReservations.Count >= 4)
            {
                throw new InvalidOperationException("Maximum 4 reservations allowed per reference ID.");
            }

            // Insert the reservation
            _reservationCollection.InsertOne(reservation);
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
            var filter = Builders<Reservation>.Filter.And(
                Builders<Reservation>.Filter.Eq(reservation => reservation.Id, id),
                Builders<Reservation>.Filter.Gte(reservation => reservation.ReservationDate, DateTime.UtcNow.AddDays(5))
                );
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
    }

    public interface IReservationService
    {
        Reservation Create(Reservation reservation);
        List<Reservation> GetAllReservations();
        Reservation GetReservationById(ObjectId id);
        Reservation Update(ObjectId id, Reservation updatedReservation);
        Reservation Delete(ObjectId id);
    }
}
