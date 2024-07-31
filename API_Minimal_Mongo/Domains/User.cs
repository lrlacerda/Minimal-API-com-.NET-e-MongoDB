using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API_Minimal_Mongo.Domains
{
    public class User
    {
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

        [BsonElement("password")]
        public string? Password { get; set; }

        [BsonElement("additionalAttributes")]
        public Dictionary<string, string> AdditionalAttributes { get; set; }

        public User()
        {
            AdditionalAttributes = new Dictionary<string, string>();
        }
    }
}
