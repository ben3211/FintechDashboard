using System.Text.Json;
using FintechDashboard.Api.Models;

namespace FintechDashboard.Api.Services
{
    public class StockService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;
        private const string ApiKey = "QGYE23X07FA1PHRC";

        public async Task<decimal?> GetStockPrice(string symbol)
        {
            var stockData = await GetStockData(symbol);
            return stockData?.Price;
        }

        public async Task<StockData?> GetStockData(string symbol)
        {
            var url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={ApiKey}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("API response: " + content);

            try
            {
                using var doc = JsonDocument.Parse(content);
                var root = doc.RootElement;

                if (root.TryGetProperty("Global Quote", out var globalQuote))
                {
                    return ParseStockData(globalQuote);
                }
                else
                {
                    Console.WriteLine("Property 'Global Quote' not found in root element.");
                }
            }
            catch (JsonException e)
            {
                Console.WriteLine("JSON parsing error: " + e.Message);
            }

            return null;
        }

        public async Task<List<StockData>> GetMultipleStocksData(List<string> symbols)
        {
            var tasks = symbols.Select(symbol => GetStockData(symbol));
            var results = await Task.WhenAll(tasks);
            return results.Where(x => x != null).ToList()!;
        }

        private StockData? ParseStockData(JsonElement globalQuote)
        {
            try
            {
                var symbol = GetStringProperty(globalQuote, "01. symbol");
                var open = GetDecimalProperty(globalQuote, "02. open");
                var high = GetDecimalProperty(globalQuote, "03. high");
                var low = GetDecimalProperty(globalQuote, "04. low");
                var price = GetDecimalProperty(globalQuote, "05. price");
                var volume = GetLongProperty(globalQuote, "06. volume");
                var latestTradingDay = GetStringProperty(globalQuote, "07. latest trading day");
                var previousClose = GetDecimalProperty(globalQuote, "08. previous close");
                var change = GetDecimalProperty(globalQuote, "09. change");
                var changePercent = GetStringProperty(globalQuote, "10. change percent");

                if (price.HasValue)
                {
                    return new StockData
                    {
                        Symbol = symbol,
                        Open = open,
                        High = high,
                        Low = low,
                        Price = price.Value,
                        Volume = volume,
                        LatestTradingDay = latestTradingDay,
                        PreviousClose = previousClose,
                        Change = change,
                        ChangePercent = changePercent
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing stock data: {ex.Message}");
            }

            return null;
        }

        private string GetStringProperty(JsonElement element, string propertyName)
        {
            return element.TryGetProperty(propertyName, out var prop) ? prop.GetString() ?? "" : "";
        }

        private decimal? GetDecimalProperty(JsonElement element, string propertyName)
        {
            if (element.TryGetProperty(propertyName, out var prop))
            {
                var value = prop.GetString();
                if (decimal.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var result))
                {
                    return result;
                }
            }
            return null;
        }

        private long? GetLongProperty(JsonElement element, string propertyName)
        {
            if (element.TryGetProperty(propertyName, out var prop))
            {
                var value = prop.GetString();
                if (long.TryParse(value, out var result))
                {
                    return result;
                }
            }
            return null;
        }
    }
}