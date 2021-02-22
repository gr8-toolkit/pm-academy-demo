namespace DepsWebApp.Models
{
    /// <summary>
    /// Exchange operation result.
    /// </summary>
    public readonly struct ExchangeResult
    {
        /// <summary>
        /// Exchange rate.
        /// </summary>
        public decimal Rate { get; }
        
        /// <summary>
        /// Amount in source currency.
        /// </summary>
        public decimal SourceAmount { get; }

        /// <summary>
        /// Amount in destination currency.
        /// </summary>
        public decimal DestinationAmount { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rate">Set <see cref="Rate"/> value.</param>
        /// <param name="srcAmount">Set <see cref="SourceAmount"/> value.</param>
        /// <param name="destAmount">Set <see cref="DestinationAmount"/> value.</param>
        public ExchangeResult(decimal rate, decimal srcAmount, decimal destAmount)
        {
            Rate = rate;
            SourceAmount = srcAmount;
            DestinationAmount = destAmount;
        }
    }
}
