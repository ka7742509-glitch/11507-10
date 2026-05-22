using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CoffeeShop.Models;

namespace CoffeeShop
{
    public class CoffeeMachine
    {
        private const string ConfigFileName = "config.json";
        private const string SalesFileName = "sales_history.txt";
        
        private Config _config = new();
        private readonly string _dataDirectory;
        private readonly JsonSerializerOptions _jsonOptions;

        public CoffeeMachine()
        {
            _dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            Directory.CreateDirectory(_dataDirectory);

            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            LoadConfig();
        }

        private string GetConfigPath() => Path.Combine(_dataDirectory, ConfigFileName);
        private string GetSalesPath() => Path.Combine(_dataDirectory, SalesFileName);
        private string GetReportPath(DateTime date) => 
            Path.Combine(_dataDirectory, $"report_{date:yyyy_MM_dd}.json");
        
        private void LoadConfig()
        {
            var path = GetConfigPath();
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path, Encoding.UTF8);
                _config = JsonSerializer.Deserialize<Config>(json, _jsonOptions) ?? new Config();
            }
            else
            {
                _config = new Config
                {
                    DrinkPrices = new Dictionary<string, decimal>
                    {
                        {"Эспрессо", 100},
                        {"Капучино", 150},
                        {"Латте", 180}
                    },
                    Stock = new Ingredients { Water = 1000, Milk = 500, Beans = 200 }
                };
                SaveConfig();
            }
        }
        
        private void SaveConfig()
        {
            var json = JsonSerializer.Serialize(_config, _jsonOptions);
            File.WriteAllText(GetConfigPath(), json, Encoding.UTF8);
        }

        public bool SellDrink(string drinkName)
        {
            if (!_config.DrinkPrices.ContainsKey(drinkName))
            {
                Console.WriteLine($"Напиток '{drinkName}' не найден в меню.");
                return false;
            }

            if (!CheckIngredients(drinkName))
            {
                Console.WriteLine("Недостаточно ингредиентов!");
                return false;
            }

            decimal price = _config.DrinkPrices[drinkName];
            DeductIngredients(drinkName);

            var sale = new Sale
            {
                Timestamp = DateTime.Now,
                DrinkName = drinkName,
                Price = price
            };

            var logEntry = $"[{sale.Timestamp:dd.MM.yyyy HH:mm}] Продано: {sale.DrinkName}, Цена: {sale.Price}";
            File.AppendAllText(GetSalesPath(), logEntry, Encoding.UTF8);

            Console.WriteLine($"{drinkName} продан за {price} ₽");
            SaveConfig();
            return true;
        }

        private bool CheckIngredients(string drinkName)
        {
            int waterNeed = drinkName == "Эспрессо" ? 30 : 150;
            int milkNeed = drinkName == "Эспрессо" ? 0 : 100;
            int beansNeed = 20;

            return _config.Stock.Water >= waterNeed &&
                   _config.Stock.Milk >= milkNeed &&
                   _config.Stock.Beans >= beansNeed;
        }

        private void DeductIngredients(string drinkName)
        {
            int waterNeed = drinkName == "Эспрессо" ? 30 : 150;
            int milkNeed = drinkName == "Эспрессо" ? 0 : 100;
            int beansNeed = 20;

            _config.Stock.Water -= waterNeed;
            _config.Stock.Milk -= milkNeed;
            _config.Stock.Beans -= beansNeed;
        }

        public void GenerateEndOfDayReport(DateTime date)
        {
            var salesPath = GetSalesPath();
            if (!File.Exists(salesPath))
            {
                Console.WriteLine("История продаж пуста.");
                return;
            }

            var report = new DailyReport
            {
                Date = date.ToString("yyyy-MM-dd"),
                DrinksSold = new Dictionary<string, int>()
            };

            foreach (var line in File.ReadLines(salesPath, Encoding.UTF8))
            { 
                if (line.Contains(date.ToString("dd.MM.yyyy")) && line.Contains("Продано:"))
                {
                    report.SalesCount++;
                    
                    var parts = line.Split("Продано:")[1].Split(",");
                    var drinkName = parts[0].Trim();
                    var pricePart = parts[1].Replace("Цена:", "").Trim().Replace("₽", "").Trim();
                    
                    if (decimal.TryParse(pricePart, out decimal price))
                        report.TotalRevenue += price;

                    if (report.DrinksSold.ContainsKey(drinkName))
                        report.DrinksSold[drinkName]++;
                    else
                        report.DrinksSold[drinkName] = 1;
                }
            }

            var json = JsonSerializer.Serialize(report, _jsonOptions);
            var reportPath = GetReportPath(date);
            File.WriteAllText(reportPath, json, Encoding.UTF8);

            Console.WriteLine($"ОТЧЁТ ЗА {report.Date}");
            Console.WriteLine($"Выручка: {report.TotalRevenue} ₽");
            Console.WriteLine($"Продаж: {report.SalesCount}");
            Console.WriteLine("По напиткам:");
            foreach (var kvp in report.DrinksSold)
                Console.WriteLine($"   • {kvp.Key}: {kvp.Value} шт.");
            Console.WriteLine($"Отчёт сохранён: {reportPath}\n");
        }

        public void ShowStock()
        {
            Console.WriteLine("Остатки ингредиентов:");
            Console.WriteLine($"Вода: {_config.Stock.Water} мл");
            Console.WriteLine($"Молоко: {_config.Stock.Milk} мл");
            Console.WriteLine($"Зёрна: {_config.Stock.Beans} г");
            Console.WriteLine("Цены:");
            foreach (var kvp in _config.DrinkPrices)
                Console.WriteLine($"{kvp.Key}: {kvp.Value}₽");
        }
    }
}