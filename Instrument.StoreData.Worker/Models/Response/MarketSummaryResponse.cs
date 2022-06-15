namespace Instrument.StoreData.Worker.Models.Response
{
    public class MarketSummaryAndSparkRootResponse
    {
        //[JsonPropertyName("marketSummaryAndSparkResponse")]
        public MarketSummaryAndSparkResponse MarketSummaryAndSparkResponse { get; set; }
    }

    public class MarketSummaryAndSparkResponse
    {
        //[JsonPropertyName("result")]
        public List<MarketSummaryResponse> Result { get; set; }
        //[JsonPropertyName("error")]
        public object Error { get; set; }
    }

    public class MarketSummaryResponse
    {
        //[JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        //[JsonPropertyName("quoteType")]
        public string QuoteType { get; set; }
        //[JsonPropertyName("shortName")]
        public string ShortName { get; set; }
    }
}
