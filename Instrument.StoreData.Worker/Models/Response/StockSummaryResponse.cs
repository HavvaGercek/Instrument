namespace Instrument.StoreData.Worker.Models.Response
{
    public class StockSummaryResponse
    {
        public string Symbol { get; set; }
        public PriceResponse Price { get; set; }
        public SummaryProfileResponse SummaryProfile { get; set; }
        public FinancialDataResponse FinancialData { get; set; }
    }

    public class PriceResponse
    {
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public string CurrencySymbol { get; set; }
        public string Currency { get; set; }
        public string ExchangeName { get; set; }
        public RegularMarketPriceResponse RegularMarketPrice { get; set; }

    }

    public class RegularMarketPriceResponse
    {
        public double? Raw { get; set; }
    }

    public class SummaryProfileResponse
    {
        public string LongBusinessSummary { get; set; }
    }

    public class FinancialDataResponse
    {
        public string RecommendationKey { get; set; }
    }
}
