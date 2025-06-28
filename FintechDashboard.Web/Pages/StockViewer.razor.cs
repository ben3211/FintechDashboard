using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FintechDashboard.Web.Pages
{
    public partial class StockViewer : ComponentBase
    {
        [Inject] private HttpClient Http { get; set; } = default!;

        protected string Symbol { get; set; } = "AAPL";
        protected decimal? Price { get; set; }
        protected string? ErrorMessage { get; set; }

        protected async Task GetPriceAsync()
        {
            ErrorMessage = null;
            Price = null;

            if (string.IsNullOrWhiteSpace(Symbol))
            {
                ErrorMessage = "Please enter a valid stock symbol.";
                return;
            }

            try
            {
                var response = await Http.GetAsync($"api/stock/{Symbol}");
                var content = await response.Content.ReadAsStringAsync();

                // Quick debug log
                Console.WriteLine($"[DEBUG] Response content: {content}");

                if (!response.IsSuccessStatusCode || content.StartsWith("<"))
                {
                    ErrorMessage = "Error from server or unexpected content format.";
                    return;
                }

                var result = JsonSerializer.Deserialize<StockPriceResult>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result?.Price > 0)
                {
                    Price = result.Price;
                }
                else
                {
                    ErrorMessage = "Stock price not found. Please check the symbol.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
        }

        public class StockPriceResult
        {
            public string Symbol { get; set; } = string.Empty;
            public decimal Price { get; set; }
        }
    }
}
