using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TicketBookingService.Models
{
    public class Travelagent
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        
        [BsonElement("name")]
        public string Name { get; set; }
        
        [BsonElement("email")]
        public string Email { get; set; }
        
        [BsonElement("password")]
        public string Password { get; set; }
        
        [BsonElement("nic")]
        public string Nic { get; set; }
        
        [BsonElement("IsActive")]
        public bool IsActive { get; set; }

        // Constructor to initialize properties
        public Travelagent()
        {
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Nic = string.Empty;
            IsActive = true; // You can set a default value here if needed
        }
    }
}
