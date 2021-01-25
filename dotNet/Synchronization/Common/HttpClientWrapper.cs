using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class HttpClientWrapper : IDisposable
    {
        SemaphoreSlim _semaphore = new SemaphoreSlim(1);
        HttpClient _httpClient = new HttpClient();

        public void Dispose()
        {
            Console.WriteLine("Release SemaphoreSlim");
            _semaphore.Dispose();
        }

        public async Task<HttpResponseMessage> GetAsync(Uri uri)
        {
            try
            {
                await _semaphore.WaitAsync();
                return await _httpClient.GetAsync(uri);
            }
            finally
            {
                _semaphore.Release();
            }

        }
    }
}
