namespace CoffeeShop.Models
{
    public class Sale
    {
        public DateTime Timestamp { get; set; }
        public string DrinkName { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}