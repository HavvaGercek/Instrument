namespace Instrument.API.Models.Request
{
    public class SubscribePriceAlertRequestModel
    {
        public string Email { get; set; }
        public string Symbol { get; set; }
        public double Price { get; set; }
    }
}
