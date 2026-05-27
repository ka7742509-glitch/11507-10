namespace CoffeeShop.Models
{
    public class DailyReport
    {
        public string Date { get; set; } = string.Empty;
        public decimal TotalRevenue { get; set; }
        public int SalesCount { get; set; }
        public Dictionary<string, int> DrinksSold { get; set; } = new();
    }
}