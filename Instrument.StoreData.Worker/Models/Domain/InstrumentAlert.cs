using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Instrument.StoreData.Worker.Models.Domain
{
    public class InstrumentAlert
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string? Symbol { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string? Email { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double? Price { get; set; }
    }

    public class InstrumentAlertQueueModel
    {

        public string? Symbol { get; set; }

        public string? Email { get; set; }

        public double? Price { get; set; }
    }
}
