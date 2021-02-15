using System.Text.Json.Serialization;

namespace Task_1
{
    public class Result
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        
        [JsonPropertyName("error")]
        public string Error { get; set; }
        
        [JsonPropertyName("duration")]
        public string Duration { get; set; }
        
        [JsonPropertyName("primes")]
        public int[] Primes { get; set; }

        public Result(bool success, string duration, int[] primes, string error = null)
        {
            Success = success;
            Duration = duration;
            Primes = primes;
            Error = error;
        }
    }
}