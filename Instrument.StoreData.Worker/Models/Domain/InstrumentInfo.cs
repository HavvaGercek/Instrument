using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
namespace Instrument.API.Models.Domain
{
    public class InstrumentInfo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("Symbol")]
        public string Symbol { get; set; }


        [BsonRepresentation(BsonType.String)]
        [BsonElement("QuoteType")]
        public string QuoteType { get; set; }


        [BsonRepresentation(BsonType.String)]
        [BsonElement("ShortName")]
        public string ShortName { get; set; }
       
    }

    public class MongoDocument<T> where T : MongoDocument<T>
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public MongoDocument()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                MapAll(Map);
            }
        }

        public virtual void Map(BsonClassMap<T> bsonClass)
        {
            bsonClass.AutoMap();
        }

        private static void MapAll(Action<BsonClassMap<T>> action)
        {
            var bsonClassMap = new BsonClassMap<T>();
            action(bsonClassMap);
            BsonClassMap.RegisterClassMap(action);
        }
    }
}
