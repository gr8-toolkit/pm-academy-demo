using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using TestableWebApi;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace TestableWebApiTests
{
    public class TestableWebApiTest
    {
        [Fact]
        public async Task CorrelationIdInResponseTest()
        {
            var host = await CreateHost<TestableWebApi.Startup>(); // IDisposable
            var server = host.GetTestServer();
            server.BaseAddress = new Uri("http://example.com/");
            var testClient = server.CreateClient();

            //var testClient = host.GetTestClient();

            var responseMessage = await testClient.GetAsync("/WeatherForecast");

            var textResponse = await responseMessage.Content.ReadAsStringAsync();
            var correlationId = responseMessage.Headers.GetValues("X-Correlation-Id");

            Assert.NotNull(textResponse);
            Assert.NotNull(correlationId.Single());
        }

        [Fact]
        public async Task CorrelationIdNotInResponseTest()
        {
            var host = await CreateHost<TestableWebApi.Startup>(); // IDisposable
            var server = host.GetTestServer();
            server.BaseAddress = new Uri("http://example.com/");
            var testClient = server.CreateClient();

            var responseMessage = await testClient.GetAsync("/status"); 
            var textResponse = await responseMessage.Content.ReadAsStringAsync();
            var correlationId = responseMessage.Headers.Contains("X-Correlation-Id");

            Assert.NotNull(textResponse);
            Assert.False(correlationId);
        }

        public async Task<IHost> CreateHost<TStartup>() where TStartup : class
        {
            var host = await new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();
                    webHost.UseStartup<TStartup>();
                }).StartAsync();

            return host;
        }
    }
}
