using System.IO;
using System.Text.Json;
using GenericHost.Models;

namespace GenericHost.ApplicationServices
{
    public class JsonWorker
    {
        private readonly string _resultFilePath = "result.json";

        public JsonWorker()
        {
        }

        public void WriteResult(Result result)
        {
            var content = JsonSerializer.Serialize(result);
            File.WriteAllText(_resultFilePath, content);

        }
    }
}