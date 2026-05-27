using System;
using System.Diagnostics;
using System.Threading;

class Program
{
    static void Main()
    {
        const int size = 3_000_000;
        double[] vec1 = new double[size];
        double[] vec2 = new double[size];
        double[] resultSeq = new double[size];
        double[] resultPar = new double[size];
        
        Random rand = new Random(42);
        for (int i = 0; i < size; i++)
        {
            vec1[i] = rand.NextDouble() * 100;
            vec2[i] = rand.NextDouble() * 100;
        }

        Stopwatch swSeq = Stopwatch.StartNew();
        for (int i = 0; i < size; i++)
        {
            resultSeq[i] = vec1[i] * vec2[i];
        }
        swSeq.Stop();
        Console.WriteLine($"Последовательное выполнение: {swSeq.ElapsedMilliseconds} мс");

        int threadCount = Environment.ProcessorCount;
        int chunkSize = size / threadCount;
        Thread[] threads = new Thread[threadCount];

        Stopwatch swPar = Stopwatch.StartNew();
        for (int t = 0; t < threadCount; t++)
        {
            int start = t * chunkSize;
            int end = (t == threadCount - 1) ? size : start + chunkSize;

            threads[t] = new Thread(() =>
            {
                for (int i = start; i < end; i++)
                {
                    resultPar[i] = vec1[i] * vec2[i];
                }
            });
            threads[t].Start();
        }

        foreach (Thread thread in threads)
        {
            thread.Join();
        }
        swPar.Stop();
        Console.WriteLine($"Параллельное выполнение ({threadCount} потоков): {swPar.ElapsedMilliseconds} мс");

        bool isCorrect = true;
        for (int i = 0; i < size; i++)
        {
            if (Math.Abs(resultSeq[i] - resultPar[i]) > 1e-9)
            {
                isCorrect = false;
                break;
            }
        }
        Console.WriteLine($"Результаты совпадают: {isCorrect}");
    }
}