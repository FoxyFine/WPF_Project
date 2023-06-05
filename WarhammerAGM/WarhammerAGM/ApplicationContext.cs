using Microsoft.EntityFrameworkCore;
using WarhammerAGM.Models;
using WarhammerAGM.Models.Arsenal;
using WarhammerAGM.Models.Arsenal.Availability;
using WarhammerAGM.Models.Arsenal.Cybernetics;
using WarhammerAGM.Models.Arsenal.Equipment;
using WarhammerAGM.Models.Arsenal.Money;
using WarhammerAGM.Models.Arsenal.Weapons;
using WarhammerAGM.Models.CriticalDamage;
using WarhammerAGM.Models.Screen;

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

        public DbSet<IncomeAndSocialClass> IncomeAndSocialClasses { get; set; } = null!;
        public DbSet<ScumbagIncome> ScumbagIncomes { get; set; } = null!;

        public DbSet<MeleeWeapon> MeleeWeapons { get; set; } = null!;
        public DbSet<RangedWeapon> RangedWeapons { get; set; } = null!;
        public DbSet<WeaponPropertie> WeaponProperties { get; set; } = null!;
        public DbSet<WeaponImprovement> WeaponImprovements { get; set; } = null!;
        public DbSet<Ammunition> Ammunitions { get; set; } = null!;

        public DbSet<Armor> Armors { get; set; } = null!;

        public DbSet<Uniform> Uniforms { get; set; } = null!;
        public DbSet<SubstancesSupplie> SubstancesSupplies { get; set; } = null!;

        public DbSet<AccessibilityEffect> AccessibilityEffects { get; set; } = null!;
        public DbSet<AccessibilityPopulation> AccessibilityPopulations { get; set; } = null!;
        public DbSet<AvailabilityTime> AvailabilityTimes { get; set; } = null!;
        public DbSet<QualityAvailability> QualityAvailabilitys { get; set; } = null!;

        public DbSet<Cybernetic> Cybernetics { get; set; } = null!;

        public DbSet<Talent> Talents { get; set; } = null!;
        public DbSet<Skill> Skills { get; set; } = null!;

        public DbSet<CriticalDamage> CriticalDamages { get; set; } = null!;

        public DbSet<World> Worlds { get; set; } = null!;
        public DbSet<TypeDamage> TypeDamages { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //Чтобы задать файл данных SQLite, в этом примере используется переопределение OnConfiguring.
        {
            optionsBuilder.UseSqlite("Data Source=AGM.db");
        }
    }
}
