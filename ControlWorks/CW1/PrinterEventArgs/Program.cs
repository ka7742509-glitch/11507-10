namespace PrinterEventArgs
{
    public class Program
    {
        public static void Main()
        {
            {
                var printer = new Printer();
        
                printer.PaperOut += (sender, e) =>
                {
                    Console.WriteLine($"Ошибка печати! Запрошено: {e.PagesRequested} стр., доступно: {e.PagesAvailable} стр.");
                };
        

                Console.WriteLine("Попытка печати 15 страниц (доступно 10):");
                printer.Print(15);
        
                Console.WriteLine();
        
                Console.WriteLine("Заправка принтера:");
                printer.Refill(10);
        
                Console.WriteLine("Повторная попытка печати 15 страниц:");
                printer.Print(15);
        
                Console.WriteLine($"Итог: осталось бумаги в принтере: {printer.GetPaperAmount()}");
                Console.WriteLine();
            }
        }
    }
}

