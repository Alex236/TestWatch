using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.SignalR;


namespace WatchServer.Hubs
{
    public class ThemesHub : Hub
    {
        private const string pathToAllProfiles = "DataAllProfiles.json";

        public async Task GetAllProfiles()
        {
            await Clients.All.SendAsync("GetAllProfiles", ReadFile(pathToAllProfiles));
        }

        public async Task UpdateAllProfiles(string content)
        {
            WriteFile(content, pathToAllProfiles);
            await Clients.All.SendAsync("GetAllProfiles", ReadFile(pathToAllProfiles));
        }

        private string ReadFile(string path)
        {
            string response = "";
            try
            {
                if (!File.Exists(path))
                {
                    File.Create(path);
                }
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        response += line;
                    }
                }
            }
            catch
            {
                response = "";
            }
            return response;
        }

        private void WriteFile(string data, string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(data);
            }
        }
    }
}
