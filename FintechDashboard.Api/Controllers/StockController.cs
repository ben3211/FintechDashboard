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

        [HttpGet("{symbol}/data")]
        public async Task<IActionResult> GetStockData(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                return BadRequest("Stock symbol cannot be empty.");
            }

            var stockData = await _stockService.GetStockData(symbol);
            if (stockData != null)
            {
                return Ok(stockData);
            }
            return NotFound($"Stock data for symbol '{symbol}' not found.");
        }

        [HttpGet("watchlist")]
        public async Task<IActionResult> GetWatchlist()
        {
            // Default watchlist with popular stocks
            var defaultSymbols = new List<string> { "AAPL", "MSFT", "GOOGL", "TSLA", "AMZN", "NVDA", "META", "NFLX" };
            
            var stocksData = await _stockService.GetMultipleStocksData(defaultSymbols);
            return Ok(stocksData);
        }

        [HttpPost("multiple")]
        public async Task<IActionResult> GetMultipleStocks([FromBody] List<string> symbols)
        {
            if (symbols == null || !symbols.Any())
            {
                return BadRequest("Symbols list cannot be empty.");
            }

            var stocksData = await _stockService.GetMultipleStocksData(symbols);
            return Ok(stocksData);
        }
    }
}
