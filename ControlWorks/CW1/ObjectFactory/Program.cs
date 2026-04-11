namespace OF
{
    class Program
    {
        static void Main(string[] args)
        {
            var tool = ObjectFactory.Create<Tool>();
            tool.Name = "Hammer";
            Console.WriteLine($"Создан объект: {tool.GetType().Name} с именем {tool.Name}");
        
            var workers = ObjectFactory.CreateList<Worker>(3);
            for (int i = 0; i < workers.Count; i++)
            {
                workers[i].Name = $"Worker_{i + 1}";
                Console.WriteLine($"Создан объект {i + 1}: {workers[i].GetType().Name} с именем {workers[i].Name}");
            }
        
            var initializedTool = ObjectFactory.CreateAndAction<Tool>(t => 
            {
                t.Name = "Screwdriver {initialized}";
            });
            Console.WriteLine($"Создан и инициализирован: {initializedTool.GetType().Name} с именем {initializedTool.Name}");
        
            var emptyList = ObjectFactory.CreateList<Worker>(-5);
            Console.WriteLine($"CreateList с отрицательным count вернул пустой список: {emptyList.Count} элементов");
        
            Console.WriteLine();
        }
    }
}