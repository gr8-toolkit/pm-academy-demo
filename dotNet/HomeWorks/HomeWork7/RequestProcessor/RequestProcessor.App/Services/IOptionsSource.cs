using System.Collections.Generic;
using System.Threading.Tasks;
using RequestProcessor.App.Models;

namespace RequestProcessor.App.Services
{
    /// <summary>
    /// Options source.
    /// </summary>
    internal interface IOptionsSource
    {
        /// <summary>
        /// Provides request-response option pairs.
        /// </summary>
        /// <returns>Returns options enumeration.</returns>
        Task<IEnumerable<(IRequestOptions, IResponseOptions)>> GetOptionsAsync();
    }
}
