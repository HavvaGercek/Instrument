namespace Instrument.API.Models.Domain
{
    public class InstrumentAlert
    {
        public string Symbol { get; set; }
        public string Email { get; set; }
        public double Price { get; set; }
    }
}
