namespace Instrument.StoreData.Worker.Settings
{
    public class FinanceApiSettings
    {
        public string ApiKey { get; set; }
        public string ApiHost { get; set; }
        public string BaseUrl { get; set; }
        public string StockSummaryUrl { get; set; }
        public string MarketSummaryUrl { get; set; }
        public int HttpClientLifeTime { get; set; }
        public int Timeout { get; set; }
    }
}
