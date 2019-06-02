using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;



namespace Watch.Services.ServerConnection
{
    public class ServerConnection : IServerConnection
    {
        public HubConnection Connection { get; } = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/watch")
                .Build();
        public bool IsBusy { get; set; }
        public bool IsConnected { get; set; }

        public async Task Connect()
        {
            if (IsConnected)
                return;
            try
            {
                await Connection.StartAsync();
                IsConnected = true;
            }
            catch
            {
                IsConnected = false;
            }
        }

        public async Task Disconnect()
        {
            if (!IsConnected)
                return;

            await Connection.StopAsync();
            IsConnected = false;
        }

        public async Task Request(string method)
        {
            try
            {
                IsBusy = true;
                await Connection.InvokeAsync(method);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task Request(string method, string content)
        {
            try
            {
                IsBusy = true;
                await Connection.InvokeAsync(method, content);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
