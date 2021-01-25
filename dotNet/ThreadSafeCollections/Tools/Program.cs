using Common.Log;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    class Program
    {
        const int _count = 100_000;
        const int _minLength = 10;
        const int _maxLength = 1000;
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz 0123456789";

        public static async Task Main(string[] args)
        {
            //await GenerateLogsFileAsync();
            //await ReadLogFilesAsync();
            Console.WriteLine("All done");
            Console.ReadKey();
        }


        static async Task GenerateLogsFileAsync()
        {
            var fileName = Path.Combine(Environment.CurrentDirectory, $"{DateTimeOffset.Now.ToUnixTimeSeconds()}-logs.json");

            using var writer = File.CreateText(fileName);
            using var json = new JsonTextWriter(writer);
            await json.WriteStartArrayAsync();

            var random = new Random();

            for (int i = 0; i < _count; i++)
            {
                var message = await Task.Run(() => new string(Enumerable.Repeat(chars, random.Next(_minLength, _maxLength)).Select(s => s[random.Next(s.Length)]).ToArray()));

                var severity = (LogSeverity)random.Next((int)LogSeverity.Debug, (int)LogSeverity.Error);

                await json.WriteWhitespaceAsync("\n");
                await json.WriteStartObjectAsync();

                await json.WritePropertyNameAsync("Message");
                await json.WriteValueAsync(message);

                await json.WritePropertyNameAsync("Level");
                await json.WriteValueAsync(severity.ToString());

                await json.WritePropertyNameAsync("Time");
                await json.WriteValueAsync(DateTimeOffset.Now.ToUnixTimeSeconds());

                await json.WriteEndObjectAsync();
            }

            await json.WriteWhitespaceAsync("\n");
            await json.WriteEndArrayAsync();
        }

        static async Task ReadLogFilesAsync()
        {
            var files = Directory.GetFiles(Environment.CurrentDirectory, "*-logs.json");

            var allLogs = files.Select(filePath => ReadLogFile(filePath));

            //await foreach (LogItem log in ReadLogFileAsync(files.First()))
            //{
            //    Console.WriteLine(log.Message);
            //}
        }

        static int ProcessLog(LogItem log)
        {
            return log.Message.Length;
        }

        static async IAsyncEnumerable<LogItem> ReadLogFileAsync(string fullPath)
        {
            using var reader = File.OpenText(fullPath);
            string line;
            LogItem item;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                try
                {
                    var commaIndex = line.IndexOf(',');
                    line = commaIndex != 0 ? line : line.Substring(1);
                    item = JsonConvert.DeserializeObject<LogItem>(line);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    item = null;
                }
                if (item != null)
                {
                    yield return item;
                }
            }
        }


        static IEnumerable<LogItem> ReadLogFile(string fullPath)
        {
            using var reader = File.OpenText(fullPath);
            string line;
            LogItem item;
            while ((line = reader.ReadLine()) != null)
            {
                try
                {
                    var commaIndex = line.IndexOf(',');
                    line = commaIndex != 0 ? line : line.Substring(1);
                    item = JsonConvert.DeserializeObject<LogItem>(line);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    item = null;
                }
                if (item != null)
                {
                    yield return item;
                }
            }
        }
    }
}
