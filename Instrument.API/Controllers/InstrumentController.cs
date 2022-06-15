using Instrument.API.Models.Request;
using Instrument.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Instrument.API.Controllers
{
    [ApiController]
    [Route("api/instrument")]
    public class InstrumentController : ControllerBase
    {
        private readonly IInstrumentService _instrumentService;
        public InstrumentController(IInstrumentService instrumentService)
        {
            _instrumentService = instrumentService;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> InstrumentList()
        {
            var response = await _instrumentService.GetInstrumentList();
            if(response == null) { return BadRequest(); }
            return  Ok(response);
        }

        [HttpGet]
        [Route("summary")]
        public async Task<IActionResult> Summary(string symbol)
        {
            var response = await _instrumentService.GetInstrumentSummary(symbol);
            if (response == null) { return BadRequest(); }
            return Ok(response);
        }

        [HttpGet]
        [Route("currentPrice")]
        public async Task<IActionResult> CurrentPrice(string symbol)
        {
            var response = await _instrumentService.GetInstrumentCurrentPrice(symbol);
            if (response == null) { return BadRequest(); }
            return Ok(response);
        }

        
        [HttpPost]
        [Route("/api/user/alert")]
        public async Task<IActionResult> SubscribePriceAlert([FromBody] SubscribePriceAlertRequestModel model)
        {
            await _instrumentService.CreateAlertForInstrument(model.Symbol, model.Email, model.Price);
            return Ok();
        }
    }
}