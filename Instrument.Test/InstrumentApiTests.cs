using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Instrument.Test
{
    public class InstrumentApiTests
    {
        [Theory]
        [InlineData("test")]
        [Trait("Method", "Get")]
        public async Task Get_Instrument_Summary_Should_Return_Bad_Request_When_Data_Is_Not_Stored(string symbol)
        {
            await using var application = new CustomWebApplicationFactory();

            using var client = application.CreateClient();

            using var response = await client.GetAsync($"/api/instrument/summary?symbol={symbol}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("test")]
        [Trait("Method", "Get")]
        public async Task Get_Instrument_Current_Price_Should_Return_Bad_Request_When_Data_Is_Not_Stored(string symbol)
        {
            await using var application = new CustomWebApplicationFactory();

            using var client = application.CreateClient();

            using var response = await client.GetAsync($"/api/instrument/currentPrice?symbol={symbol}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GET_Instruments_List_Responds_OK()
        {
            await using var application = new CustomWebApplicationFactory();

            using var client = application.CreateClient();
            using var response = await client.GetAsync("/api/instrument/list");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task POST_SubscribePriceAlert_With_NoBody_Responds_UnsupportedMediaType()
        {
            await using var application = new CustomWebApplicationFactory();

            using var jsonContent = new StringContent("");

            using var client = application.CreateClient();
            using var response = await client.PostAsync("/api/user/alert", jsonContent);

            Assert.Equal(HttpStatusCode.UnsupportedMediaType, response.StatusCode);
        }

        [Fact]
        public async Task POST_SubscribePriceAlert_With_False_Body_Responds_BadRequest()
        {
            await using var application = new CustomWebApplicationFactory();

            var model = JsonSerializer.Serialize(new { symbol = "ES=F", email = 11, price = 1500 });
            using var jsonContent = new StringContent(model, Encoding.UTF8, "application/json");

            using var client = application.CreateClient();
            using var response = await client.PostAsync("/api/user/alert", jsonContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task POST_SubscribePriceAlert_With_Body_Request_Responds_Ok()
        {
            await using var application = new CustomWebApplicationFactory();
            var model = JsonSerializer.Serialize(new { symbol = "ES=F", email = "test@test.com", price = new Random().NextDouble() });
            using var jsonContent = new StringContent(model, Encoding.UTF8, "application/json");

            using var client = application.CreateClient();
            using var response = await client.PostAsync("/api/user/alert", jsonContent);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}