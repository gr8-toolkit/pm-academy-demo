using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using DepsWebApp.Services;

namespace DepsWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatesController : ControllerBase
    {
        private readonly ILogger<RatesController> _logger;
        private readonly IRatesService _rates;

        public RatesController(
            IRatesService rates,
            ILogger<RatesController> logger)
        {
            _rates = rates;
            _logger = logger;
        }

        [HttpGet("{srcCurrency}/{dstCurrency}")]
        public async Task<ActionResult<decimal>> Get(string srcCurrency, string dstCurrency, decimal? amount)
        {
            var exchange =  await _rates.ExchangeAsync(srcCurrency, dstCurrency, amount ?? decimal.One);
            if (!exchange.HasValue)
            {
                _logger.LogDebug($"Can't exchange from '{srcCurrency}' to '{dstCurrency}'");
                return BadRequest("Invalid currency code");
            }
            return exchange.Value.DestinationAmount;
        }
    }
}
