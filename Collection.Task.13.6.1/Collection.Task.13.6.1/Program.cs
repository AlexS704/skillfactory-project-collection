/*
 *Задание 13.6.1
 * Наша задача — сравнить производительность вставки в List<T> и LinkedList<T>.Для этого используйте уже знакомый вам StopWatch.
 * На примере этого текста, выясните, какие будут различия между этими коллекциями.
 */

using System.Diagnostics;
using System.Security.Claims;

//Путь до рабочего стола текущего пользователя
string desktopPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Desktop");
const string fileName = "input.txt";
string filePath = Path.Combine(desktopPath, fileName);

if (File.Exists(filePath))
{
    Console.WriteLine($"Файл '{fileName}' найден на рабочем столе.");
    Console.WriteLine($"Полный путь: {filePath}");

    // Дополнительная информация о файле
    FileInfo fileInfo = new FileInfo(filePath);
    Console.WriteLine($"Размер: {fileInfo.Length} байт");
    Console.WriteLine($"Создан: {fileInfo.CreationTime}");    
}
else
{
    Console.WriteLine($"Файл '{fileName}' отсутствует на рабочем столе.");
}

const int testIterations = 10;

CreateTestFileIfNeeded(fileName);
string[] lines = File.ReadAllLines(filePath);

Console.WriteLine($"Тестирование производительности ({testIterations} итераций)");
Console.WriteLine($"Количество строк: {lines.Length}\n");

//Многократное тестирование для получения более точных результатов
double listEndTotalMs = 0;
double listStartTotalMs = 0;
double linkedListEndTotalMs = 0;
double linkedListStartTotalMs = 0;

for (int i = 0; i < testIterations; i++)
{
    Console.WriteLine($"\n--- Итерация {i + 1}: ---");

    //Очистка памяти перед каждым тестом
    GC.Collect();
    GC.WaitForPendingFinalizers();
    
    var results = RunPerformanceTest(lines);

    listEndTotalMs += results.ListEndMs;
    listStartTotalMs += results.ListStartMs;
    linkedListEndTotalMs += results.LinkedListEndMs;
    linkedListStartTotalMs += results.LinkedListStartMs;
}

//Вывод средних результатов
Console.WriteLine("\n=== СРЕДНИЕ РЕЗУЛЬТАТЫ (мс)===");
Console.WriteLine($"List<T> - Вставка в конец: {listEndTotalMs / testIterations:F3} мс");
Console.WriteLine($"List<T> - Вставка в начало: {listStartTotalMs / testIterations:F3} мс");
Console.WriteLine($"LinkedList<T> - Вставка в конец: {linkedListEndTotalMs / testIterations:F3} мс");
Console.WriteLine($"LinkedList<T> - Вставка в начало: {linkedListStartTotalMs / testIterations:F3} мс");

// Сравнение производительности
Console.WriteLine("\n=== СРАВНЕНИЕ ===");
Console.WriteLine($"List в конец vs LinkedList в конец: " +
                 $"{(listEndTotalMs / testIterations) / (linkedListEndTotalMs / testIterations):F2}x");
Console.WriteLine($"List в начало vs LinkedList в начало: " +
                 $"{(listStartTotalMs / testIterations) / (linkedListStartTotalMs / testIterations):F2}x");

static (double ListEndMs, double ListStartMs, double LinkedListEndMs, double LinkedListStartMs)
       RunPerformanceTest(string[] lines)
{
    var listResult = TestList(lines);
    var linkedListResult = TestLinkedList(lines);

    Console.WriteLine($"List: конец={listResult.EndMs:F3}мс, начало={listResult.StartMs:F3}мс");
    Console.WriteLine($"LinkedList: конец={linkedListResult.EndMs:F3}мс, начало={linkedListResult.StartMs:F3}мс");

    return (listResult.EndMs, listResult.StartMs,
            linkedListResult.EndMs, linkedListResult.StartMs);
}
static (double EndMs, double StartMs) TestList(string[] lines)
{
    List<string> list = new List<string>();
    Stopwatch stopwatch = new Stopwatch();

    // Вставка в конец
    stopwatch.Start();
    foreach (string line in lines)
    {
        list.Add(line);
    }
    stopwatch.Stop();
    double endMs = stopwatch.Elapsed.TotalMilliseconds;

    // Вставка в начало
    list.Clear();
    stopwatch.Restart();
    foreach (string line in lines)
    {
        list.Insert(0, line);
    }
    stopwatch.Stop();
    double startMs = stopwatch.Elapsed.TotalMilliseconds;

    return (endMs, startMs);
}
static (double EndMs, double StartMs) TestLinkedList(string[] lines)
{
    LinkedList<string> linkedList = new LinkedList<string>();
    Stopwatch stopwatch = new Stopwatch();

    // Вставка в конец
    stopwatch.Start();
    foreach (string line in lines)
    {
        linkedList.AddLast(line);
    }
    stopwatch.Stop();
    double endMs = stopwatch.Elapsed.TotalMilliseconds;

    // Вставка в начало
    linkedList.Clear();
    stopwatch.Restart();
    foreach (string line in lines)
    {
        linkedList.AddFirst(line);
    }
    stopwatch.Stop();
    double startMs = stopwatch.Elapsed.TotalMilliseconds;

    return (endMs, startMs);
}
static void CreateTestFileIfNeeded(string filePath)
{
    if (!File.Exists(filePath))
    {
        var lines = Enumerable.Range(1, 50000)
            .Select(i => $"Строка {i}: {Guid.NewGuid()} - Тестирование производительности коллекций")
            .ToArray();

        File.WriteAllLines(filePath, lines);
        Console.WriteLine($"Создан тестовый файл с {lines.Length} строками.");
    }
}