using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Watch.Models;


namespace WatchServer.Hubs
{
    public class ThemesHub : Hub
    {
        private const string pathToData = "../Data.json";
        public async Task GetAllProfilesWithCurrent()
        {
            await Clients.All.SendAsync("GetAllProfilesWithCurrent", GetCurrentProfile());
        }

        public async Task UpdateAllData(string content)
        {
            SetCurrentProfile(content);
            await Clients.All.SendAsync("GetAllProfilesWithCurrent", GetCurrentProfile());
        }

        private string GetCurrentProfile()
        {
            if (!File.Exists(pathToData))
            {
                File.Create(pathToData);
            }
            string[] strings = File.ReadAllLines(pathToData);
            string result = strings == null ? "" : string.Concat(strings);
            return result;
        }

        private void SetCurrentProfile(string allProfilesWithCurrent)
        {
            if (!File.Exists(pathToData))
            {
                File.Create(pathToData);
            }
            using (StreamWriter sw = new StreamWriter(pathToData))
            {
                sw.WriteLine(JsonConvert.SerializeObject(allProfilesWithCurrent));
            }
        }
    }
}
