namespace DepsWebApp.Models
{
    /// <summary>
    /// Currency rate model.
    /// </summary>
    public class CurrencyRate
    {
        /// <summary>
        /// Currency code (ISO).
        /// </summary>
        public string Currency { get; set; }
        
        /// <summary>
        /// Currency rate related to base currency.
        /// </summary>
        public decimal Rate { get; set; }
    }
}
