using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        string path = @"C:\Windows";
        
        if (!Directory.Exists(path))
        {
            Console.WriteLine("Папка не найдена");
            return;
        }

        try
        {
            var result = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly)
                .Select(f => new FileInfo(f))
                .Where(fi => fi.Exists)
                .GroupBy(fi => fi.Extension.ToLower())
                .Select(g => g.OrderByDescending(f => f.Length).First())
                .OrderByDescending(fi => fi.Length)
                .Take(5);

            foreach (var file in result)
            {
                double mb = Math.Round(file.Length / (1024.0 * 1024.0), 1);
                Console.WriteLine($"{file.Extension}: {file.Name} - [{mb} MB]");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}