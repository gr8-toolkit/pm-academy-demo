using System;
using System.Net.Http;
using System.Threading.Tasks;
using RequestProcessor.App.Models;

namespace RequestProcessor.App.Services
{
    /// <summary>
    /// Request handler.
    /// Thin typed HTTP client (not a leaky abstraction).
    /// </summary>
    internal interface IRequestHandler
    {
        /// <summary>
        /// Handle requests.
        /// </summary>
        /// <param name="requestOptions">Request parameters.</param>
        /// <returns>
        /// Returns response details.
        /// </returns>
        /// <exception cref="ArgumentNullException"><see cref="requestOptions"/> is missing.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><see cref="requestOptions"/> is not valid.</exception>
        /// <exception cref="InvalidOperationException">Invalid client state.</exception>
        /// <exception cref="HttpRequestException">Internal client exception.</exception>
        /// <exception cref="TaskCanceledException">Internal client exception.</exception>
        Task<IResponse> HandleRequestAsync(IRequestOptions requestOptions);
    }
}
