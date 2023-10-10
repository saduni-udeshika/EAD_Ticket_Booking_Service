using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TicketBookingService.Models
{
    public class Backoffice
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        
        [BsonElement("username")] // Change attribute name to "username"
        public string Username { get; set; }
        
        [BsonElement("password")] // Change attribute name to "password"
        public string Password { get; set; }
        
        [BsonElement("nic")] // Change attribute name to "nic"
        public string Nic { get; set; }
        
        [BsonElement("IsActive")]
        public bool IsActive { get; set; }

        // Constructor to initialize properties
        public Backoffice()
        {
            Username = string.Empty;
            Password = string.Empty;
            Nic = string.Empty;
            IsActive = true; // You can set a default value here if needed
        }
    }
}
