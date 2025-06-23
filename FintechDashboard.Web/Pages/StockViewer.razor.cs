using System.Net.Http.Json;

namespace FintechDashboard.Web.Pages
{
    public partial class StockViewer
    {
    private string Symbol = "AAPL";
        private decimal? Price = null;
        private bool Loading = false;
        private bool HasError = false;

        private async Task FetchPrice()
        {
            Loading = true;
            HasError = false;
            Price = null;

            try
            {
                var result = await Http.GetFromJsonAsync<StockResult>($"https://localhost:7283/api/stock/{Symbol}");
                Price = result?.Price;
            }
            catch
            {
                HasError = true;
            }
            finally
            {
                Loading = false;
            }
        }

        public class StockResult
        {
            public string? Symbol { get; set; }
            public decimal Price { get; set; }
        }
    }
}
