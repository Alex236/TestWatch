using Microsoft.EntityFrameworkCore;

namespace WatchServer.DbThemes
{
    public class DbThemeContext : DbContext
    {
        public DbSet<ThemeModel> Themes;

        public DbThemeContext(DbContextOptions<DbThemeContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
