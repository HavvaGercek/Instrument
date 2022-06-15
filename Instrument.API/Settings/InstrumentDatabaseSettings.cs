namespace Instrument.API.Settings
{
    public class InstrumentDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string MarketSummaryCollectionName { get; set; } = null!;
        public string AlertCollectionName { get; set; } = null!;

        public string StockSummaryCollectionName { get; set; } = null!;
    }
}
