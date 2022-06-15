using System.Text.Json;

namespace Instrument.StoreData.Worker.Helpers
{
    public static class JsonSerializerHelper
    {
        public static JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        { 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}
