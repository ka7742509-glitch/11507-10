using System.Text.Json.Serialization;

namespace CoffeeShop.Models
{
    public class Config
    {
        public Dictionary<string, decimal> DrinkPrices { get; set; } = new();
        public Ingredients Stock { get; set; } = new();
    }

    public class Ingredients
    {
        public int Water { get; set; }
        public int Milk { get; set; }
        public int Beans { get; set; }
    }
}