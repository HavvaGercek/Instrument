using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Instrument.API.Models.Domain
{
    public class InstrumentInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string? QuoteType { get; set; }
        public string? ShortName { get; set; }
       
    }
}
