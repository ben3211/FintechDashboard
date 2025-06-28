using FintechDashboard.Web.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace FintechDashboard.Web.Pages
{
    public partial class StockDashboard : ComponentBase
    {

        private List<StockData>? Stocks { get; set; }
        private bool IsLoading { get; set; } = true;
        private string? ErrorMessage { get; set; }
        private DateTime LastUpdated { get; set; } = DateTime.Now;
        private bool ShowAddStockModal { get; set; } = false;
        private string NewStockSymbol { get; set; } = "";
        private List<string> Watchlist { get; set; } = new List<string> { "AAPL", "MSFT", "GOOGL", "TSLA", "AMZN", "NVDA", "META", "NFLX" };

        protected override async Task OnInitializedAsync()
        {
            await LoadStockData();
        }

        private async Task LoadStockData()
        {
            IsLoading = true;
            ErrorMessage = null;

            try
            {
                var response = await Http.GetAsync("api/stock/watchlist");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    
                    Stocks = JsonSerializer.Deserialize<List<StockData>>(content, options);
                    LastUpdated = DateTime.Now;
                }
                else
                {
                    ErrorMessage = $"Failed to load stock data. Status: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading stock data: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }

        private async Task RefreshData()
        {
            await LoadStockData();
        }

        private void AddStock()
        {
            ShowAddStockModal = true;
            NewStockSymbol = "";
        }

        private void CloseAddStockModal()
        {
            ShowAddStockModal = false;
            NewStockSymbol = "";
        }

        private async Task ConfirmAddStock()
        {
            if (string.IsNullOrWhiteSpace(NewStockSymbol))
            {
                await JSRuntime.InvokeVoidAsync("alert", "Please enter a valid stock symbol.");
                return;
            }

            var symbol = NewStockSymbol.Trim().ToUpper();
            
            if (Watchlist.Contains(symbol))
            {
                await JSRuntime.InvokeVoidAsync("alert", $"Stock {symbol} is already in your watchlist.");
                return;
            }

            try
            {
                // Test if the stock exists by trying to get its data
                var response = await Http.GetAsync($"api/stock/{symbol}/data");
                
                if (response.IsSuccessStatusCode)
                {
                    Watchlist.Add(symbol);
                    CloseAddStockModal();
                    await LoadStockData(); // Reload with new stock
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("alert", $"Stock symbol {symbol} not found. Please check the symbol and try again.");
                }
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", $"Error adding stock: {ex.Message}");
            }
        }

        private async Task RemoveStock(string symbol)
        {
            if (Watchlist.Contains(symbol))
            {
                Watchlist.Remove(symbol);
                await LoadStockData();
            }
        }
    }
} 