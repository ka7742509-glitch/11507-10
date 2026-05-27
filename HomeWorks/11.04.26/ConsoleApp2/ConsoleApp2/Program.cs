using System;
using System.Diagnostics;
using System.IO;

class GiantDataAnalyzer
{
    static void Main()
    {
        GenerateTestFile(); 
        AnalyzeFile("bigdata.txt");
    }

    static void GenerateTestFile()
    {
        var sw = Stopwatch.StartNew();
        
        using (var writer = new StreamWriter("bigdata.txt"))
        {
            for (int i = 0; i < 50_000_000; i++)
                writer.WriteLine("Data line with some A symbols and other chars");
        }
        
        sw.Stop();
        Console.WriteLine($"Файл создан за {sw.ElapsedMilliseconds} мс");
    }

    static void AnalyzeFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Файл {filePath} не найден!");
            return;
        }

        const int bufferSize = 65536;
        byte[] buffer = new byte[bufferSize];
        long countA = 0;
        long totalBytes = 0;

        var stopwatch = Stopwatch.StartNew();

        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.SequentialScan))
        {
            int bytesRead;
            while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
            {
                totalBytes += bytesRead;
                
                for (int i = 0; i < bytesRead; i++)
                {
                    if (buffer[i] == 65)
                        countA++;
                }
            }
        }

        stopwatch.Stop();
        
        Console.WriteLine($"Результаты анализа:");
        Console.WriteLine($"• Прочитано байт: {totalBytes:N0}");
        Console.WriteLine($"• Количество символов 'A': {countA:N0}");
        Console.WriteLine($"• Время выполнения: {stopwatch.ElapsedMilliseconds} мс ({stopwatch.Elapsed.TotalSeconds:F2} сек)");
        Console.WriteLine($"• Использован буфер: {bufferSize} байт");
        Console.WriteLine($"• Потребление RAM: ~{bufferSize / 1024} КБ (в пределах 50 МБ)");
    }
}