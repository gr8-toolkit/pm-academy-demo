using System.Text.Json.Serialization;

namespace DepsWebApp.Contracts
{
    /// <summary>
    /// NBU provider native data model for currency rate.
    /// </summary>
    public class NbuRate
    {
        /// <summary>
        /// Currency rate related to base currency (UAH).
        /// </summary>
        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }

        /// <summary>
        /// Currency code.
        /// </summary>
        [JsonPropertyName("cc")]
        public string Currency { get; set; }
    }
}