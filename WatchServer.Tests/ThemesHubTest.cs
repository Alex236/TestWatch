using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR.Client;
using WatchServer.Hubs;


namespace WatchServer.Tests
{
    public class ThemesHubTest
    {
        [Theory]
        [InlineData("dsfafaf", "")]
        public async Task AddThemeTest(string input, string expectedOutput)
        {
            var response = await StartTestServer(input, "AddTheme");
            Assert.Equal(response, expectedOutput);
        }

        [Theory]
        [InlineData("dsfafaf", "")]
        public async Task RemoveThemeTest(string input, string expectedOutput)
        {
            var response = await StartTestServer(input, "RemoveTheme");
            Assert.Equal(response, expectedOutput);
        }

        private async Task<string> StartTestServer(string input, string method)
        {
            var response = string.Empty;
            TestServer server = null;
            var webHostBuilder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSignalR();
                })
                .Configure(app =>
                {
                    app.UseSignalR(routes => routes.MapHub<ThemesHub>("/watch"));
                });
            server = new TestServer(webHostBuilder);
            var connection = new HubConnectionBuilder()
                .WithUrl(
                    "https://localhost:5001/watch",
                    o => o.HttpMessageHandlerFactory = _ => server.CreateHandler())
                .Build();
            connection.On<string>(method, msg =>
            {
                response = msg;
            });
            await connection.StartAsync();
            await connection.InvokeAsync(method, input);
            return response;
        }
    }
}
