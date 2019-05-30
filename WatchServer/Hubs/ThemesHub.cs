using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using WatchServer.DbThemes;
//susing Watch.Models;


namespace WatchServer.Hubs
{
    public class ThemesHub : Hub
    {
        private DbThemeContext themes;

        public ThemesHub(DbThemeContext dbThemes)
        {
            themes = dbThemes;
        }

        public async Task AddTheme(string themeMessege)
        {
            //Theme theme;
            try
            {
            //    theme = JsonConvert.DeserializeObject<Theme>(themeMessege);
            }
            catch
            {
                themeMessege = "";
            }
            finally
            {
                await Clients.All.SendAsync("AddTheme", themeMessege);
            }
        }

        public async Task RemoveTheme(string themeMessege)
        {
            await Clients.All.SendAsync("RemoveTheme", "");
        }
    }
}
