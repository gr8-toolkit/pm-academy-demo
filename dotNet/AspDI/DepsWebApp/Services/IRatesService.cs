using System.Threading.Tasks;
using DepsWebApp.Models;

namespace DepsWebApp.Services
{
    /// <summary>
    /// Currency rates service.
    /// </summary>
    public interface IRatesService
    {
        /// <summary>
        /// Exchanges given amount from source currency to destination currency.
        /// </summary>
        /// <param name="srcCurrency">Source currency</param>
        /// <param name="destCurrency">Destination currency</param>
        /// <param name="amount">Amount of funds.</param>
        /// <returns>Returns exchange result or <c>null</c> if source or destination currency wasn't found.</returns>
        public Task<ExchangeResult?> ExchangeAsync(string srcCurrency, string destCurrency, decimal amount);

        /// <summary>
        /// Actualize rates.
        /// Kind of abstraction leak.
        /// </summary>
        /// <returns>Returns awaiter</returns>
        public Task ActualizeRatesAsync();
    }
}
