using Microsoft.EntityFrameworkCore;
using WarhammerAGM.Models;

namespace WarhammerAGM
{
    public class ApplicationContext : DbContext
    {
        public DbSet<BestiaryCreature> BestiaryCreatures { get; set; } = null!; // сообщает EF Core, какие сущности C# следует сопоставить с базой данных
        public DbSet<Weapon> Weapons { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //Чтобы задать файл данных SQLite, в этом примере используется переопределение OnConfiguring.
        {
            optionsBuilder.UseSqlite("Data Source=Bestiary.db");
            optionsBuilder.UseSqlite("Data Source=Weapons.db");
        }
    }
}
