namespace FintechDashboard.Web.Models
{
    public class StockData
    {
        public string Symbol { get; set; } = "";
        public decimal? Open { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal Price { get; set; }
        public long? Volume { get; set; }
        public string LatestTradingDay { get; set; } = "";
        public decimal? PreviousClose { get; set; }
        public decimal? Change { get; set; }
        public string ChangePercent { get; set; } = "";

        public bool IsPositiveChange => Change > 0;
        public bool IsNegativeChange => Change < 0;
        public string ChangeColor => IsPositiveChange ? "text-success" : IsNegativeChange ? "text-danger" : "text-muted";
        public string ChangeIcon => IsPositiveChange ? "↗" : IsNegativeChange ? "↘" : "→";
        public string ChangeBgColor => IsPositiveChange ? "bg-success-subtle" : IsNegativeChange ? "bg-danger-subtle" : "bg-secondary-subtle";
    }
} 