using FintechDashboard.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FintechDashboard.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {

        private readonly StockService _stockService;
        public StockController(StockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet("{symbol}")]
        public async Task<IActionResult> GetStockPrice(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                return BadRequest("Stock symbol cannot be empty.");
            }

            var price = await _stockService.GetStockPrice(symbol);
            if (price.HasValue)
            {
                return Ok(new { Symbol = symbol, Price = price.Value });
            }
            return NotFound($"Stock price for symbol '{symbol}' not found.");
        }

    }
}
