//using Watch.Models;


namespace WatchServer.DbThemes
{
    public class ThemeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //public static explicit operator Theme (ThemeModel theme)
        //{
        //    return new Theme()
        //    {
        //        Name = theme.Name
        //    };
        //}
    }
}
