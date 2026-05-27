using CoffeeShop;

class Program
{
    static void Main(string[] args)
    {
        var machine = new CoffeeMachine();
        bool running = true;

        Console.WriteLine("Кофемашина");

        while (running)
        {
            Console.WriteLine("МЕНЮ");
            Console.WriteLine("1. Продать напиток");
            Console.WriteLine("2. Показать остатки и цены");
            Console.WriteLine("3. Конец смены (отчёт)");
            Console.WriteLine("4. Редактировать конфигурацию");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1":
                    machine.ShowStock();
                    Console.Write("Название напитка: ");
                    var drink = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(drink))
                        machine.SellDrink(drink);
                    break;

                case "2":
                    machine.ShowStock();
                    break;

                case "3":
                    Console.Write("Дата отчёта (нажмите Enter для сегодня): ");
                    var input = Console.ReadLine()?.Trim();
                    var date = string.IsNullOrEmpty(input) ? DateTime.Today : DateTime.Parse(input);
                    machine.GenerateEndOfDayReport(date);
                    break;

                case "4":
                    EditConfig(machine);
                    break;

                case "0":
                    running = false;
                    Console.WriteLine("Приходите еще!");
                    break;

                default:
                    Console.WriteLine("Неверный выбор");
                    break;
            }
        }
    }

    static void EditConfig(CoffeeMachine machine)
    { 
        Console.WriteLine("Редактирование конфигурации");
        Console.WriteLine("1. Добавить/изменить цену напитка");
        Console.WriteLine("2. Пополнить ингредиенты");
        Console.Write("Выбор: ");

        if (Console.ReadLine()?.Trim() == "1")
        {
            Console.Write("Название напитка: ");
            var name = Console.ReadLine()?.Trim();
            Console.Write("Цена: ");
            if (!string.IsNullOrEmpty(name) && decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("(В полной версии: цена обновлена в config.json)");
            }
        }
        else if (Console.ReadLine()?.Trim() == "2")
        {
            Console.WriteLine("(В полной версии: реализуйте пополнение ингредиентов)");
        }
    }
}