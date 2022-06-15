using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Instrument.API.Models.Domain
{
    public class InstrumentSummary
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("ShortName")]
        public string? ShortName { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("Symbol")]
        public string Symbol { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("LongName")]
        public string? LongName { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("CurrencySymbol")]
        public string? CurrencySymbol { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("Currency")]
        public string? Currency { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("ExchangeName")]
        public string? ExchangeName { get; set; }

        [BsonRepresentation(BsonType.Double)]
        [BsonElement("Price")]
        public double? Price { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("Summary")]
        public string? Summary { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("Recommendation")]
        public string? Recommendation { get; set; }
    }

}
