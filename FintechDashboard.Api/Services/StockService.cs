using System.Text.Json;

namespace FintechDashboard.Api.Services
{
    public class StockService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;
        private const string ApiKey = "QGYE23X07FA1PHRC";

        public async Task<decimal?> GetStockPrice(string symbol)
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

                // Alpha Vantage sometimes wraps keys with whitespace or casing issues
                if (root.TryGetProperty("Global Quote", out var globalQuote))
                {
                    if (globalQuote.TryGetProperty("05. price", out var priceProperty))
                    {
                        var rawPrice = priceProperty.GetString();
                        Console.WriteLine("Raw price value: " + rawPrice);

                        if (decimal.TryParse(rawPrice, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var price))
                        {
                            return price;
                        }
                        else
                        {
                            Console.WriteLine("Failed to parse price.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Property '05. price' not found in 'Global Quote'.");
                    }
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
    }
}