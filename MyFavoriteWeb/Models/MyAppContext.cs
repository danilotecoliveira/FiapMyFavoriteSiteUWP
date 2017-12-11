using Microsoft.EntityFrameworkCore;

namespace MyFavoriteWeb.Models
{
    public class MyAppContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Site> Sites { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=MyFavoriteWeb251.db");
        }
    }
}
