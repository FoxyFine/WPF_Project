using Microsoft.EntityFrameworkCore;
using WarhammerAGM.Models;

namespace WarhammerAGM
{
    public class ApplicationContext : DbContext
    {
        public DbSet<CreatureBase> CreatureBases { get; set; } = null!;

        public DbSet<BestiaryCreature> BestiaryCreatures { get; set; } = null!; // сообщает EF Core, какие сущности C# следует сопоставить с базой данных
        public DbSet<Character> Characters { get; set; } = null!;

        public DbSet<Initiative> Initiatives { get; set; } = null!;
        public DbSet<TemporaryInitiative> TemporaryInitiatives { get; set; } = null!;

        public DbSet<DeathListInitiative> DeathListInitiatives { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //Чтобы задать файл данных SQLite, в этом примере используется переопределение OnConfiguring.
        {
            optionsBuilder.UseSqlite("Data Source=AGM.db");
        }
    }
}
