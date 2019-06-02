using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;


namespace Watch.Services.ServerConnection
{
    public interface IServerConnection
    {
        HubConnection Connection { get; }
        bool IsBusy { get; set; }
        bool IsConnected { get; set; }
        Task Connect();
        Task Disconnect();
        Task Request(string method);
        Task Request(string method, string content);
    }
}
