using System;
using System.Threading.Tasks;
using RequestProcessor.App.Exceptions;
using RequestProcessor.App.Models;

namespace RequestProcessor.App.Services
{
    /// <summary>
    /// Request performer.
    /// </summary>
    internal interface IRequestPerformer
    {
        /// <summary>
        /// Performs requests and manage response.
        /// </summary>
        /// <param name="requestOptions">Required request options.</param>
        /// <param name="responseOptions">Required response options</param>
        /// <returns>
        /// Returns <c>true</c> if request was successfully sent and response was stored.
        /// Returns <c>false</c> if request was sent, but response has some errors.
        /// </returns>
        /// <exception cref="ArgumentNullException">One of required parameters are missing.</exception>
        /// <exception cref="ArgumentOutOfRangeException">One of parameters is not valid.</exception>
        /// <exception cref="PerformException">Critical performer error. See inner exception for more details.</exception>
        Task<bool> PerformRequestAsync(IRequestOptions requestOptions, IResponseOptions responseOptions);
    }
}
