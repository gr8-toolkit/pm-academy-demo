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
    public class TestServerIntro
    {
        [Fact]
        public async Task PingTest()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();
                    webHost.Configure(app => app.Run(async context =>
                    await context.Response.WriteAsync("ping")));
                });
            var host = await hostBuilder.StartAsync();
            var client = host.GetTestClient();
            var response = await client.GetAsync("/");
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal("ping", responseString);
        }
    }
}
