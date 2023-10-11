using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Interfaces.DTO
{
    public class PollDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string pollName { get; set; }
        public List<string> options { get; set; }
        public List<int> votes { get; set; }
        public bool active { get; set; }
        public bool visible { get; set; }
        public DateTime dateCreated { get; set; }
    }
}
