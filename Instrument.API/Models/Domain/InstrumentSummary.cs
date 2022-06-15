using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Instrument.API.Models.Domain
{
    public class InstrumentSummary
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string? ShortName { get; set; }
        public string? LongName { get; set; }
        public string? CurrencySymbol { get; set; }
        public string? Currency { get; set; }
        public string ExchangeName { get; set; }
        public double? Price { get; set; }
        public string? Summary { get; set; }
        public string? Recommendation { get; set; }
        public string Symbol { get; set; }
    }

}
