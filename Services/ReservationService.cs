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
            // Parse ReservationDate as a DateTime
            if (!DateTime.TryParse(reservation.ReservationDate, out var reservationDate))
            {
                throw new ArgumentException("Invalid date format.");
            }

            // Calculate the difference between the reservation date and the current date
            var dateDifference = reservationDate.Date - DateTime.Now.Date;

            // Check if the reservation date is within 30 days from the booking date
            if (dateDifference.Days > 30)
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
    var existingReservation = _reservationCollection.Find(reservation => reservation.Id == id).FirstOrDefault();

    if (existingReservation == null)
    {
        return null; // Reservation not found
    }

    // Convert the existing reservation's ReservationDate to DateTime
    if (!DateTime.TryParse(existingReservation.ReservationDate, out var existingReservationDate))
    {
        throw new ArgumentException("Invalid date format.");
    }

    // Check if the existing reservation can be updated (at least 5 days before the reservation date)
    var minAllowedReservationDate = DateTime.UtcNow.AddDays(5);
    if (existingReservationDate < minAllowedReservationDate)
    {
        throw new ArgumentException("Reservation can only be updated at least 5 days before the reservation date.");
    }

    // Update the reservation properties
    existingReservation.PhoneNumber = updatedReservation.PhoneNumber;
    existingReservation.ReservationDate = updatedReservation.ReservationDate;
    existingReservation.Destination = updatedReservation.Destination;
    existingReservation.Time = updatedReservation.Time;

    // Save the updated reservation
    _reservationCollection.ReplaceOne(reservation => reservation.Id == id, existingReservation);

    return existingReservation;
}

   public Reservation Cancel(ObjectId id)
        {
            var existingReservation = _reservationCollection.Find(reservation => reservation.Id == id).FirstOrDefault();

            if (existingReservation == null)
            {
                return null; // Reservation not found
            }

            // Convert the existing reservation's ReservationDate to DateTime
            if (!DateTime.TryParse(existingReservation.ReservationDate, out var existingReservationDate))
            {
                throw new ArgumentException("Invalid date format.");
            }

            // Calculate the time difference between the reservation date and the current date
            TimeSpan timeDifference = existingReservationDate - DateTime.UtcNow;

            // Check if the reservation can be canceled (at least 5 days before the reservation date)
            if (timeDifference.TotalDays < 5)
            {
                throw new ArgumentException("Reservations can only be canceled at least 5 days before the reservation date.");
            }

            // Remove the reservation from the database
            _reservationCollection.DeleteOne(reservation => reservation.Id == id);

            return existingReservation;
        }
      
    
 } 

    public interface IReservationService
    {
        Reservation Create(Reservation reservation);
        List<Reservation> GetAllReservations();
        Reservation GetReservationById(ObjectId id);
        Reservation Update(ObjectId id, Reservation updatedReservation);
        Reservation Cancel(ObjectId id);
    }
}
