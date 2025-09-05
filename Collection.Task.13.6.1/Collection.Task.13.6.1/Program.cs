/*
 *Задание 13.6.1
 * Наша задача — сравнить производительность вставки в List<T> и LinkedList<T>.Для этого используйте уже знакомый вам StopWatch.
 * На примере этого текста, выясните, какие будут различия между этими коллекциями.
 */
using System.Reflection.PortableExecutable;

string desktopPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Desktop");
string fileName = "input.txt";
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

List<string> files = new List<string>();

using (StreamReader reader = new StreamReader(filePath, FileMode.Open))
{
    int byteValue;
    while ((byteValue = reader.Read()) != -1)
    {
        files.Add(reader.ReadToEnd());
    }
}
Console.WriteLine($"Прочитано байт: {files.Count}");
