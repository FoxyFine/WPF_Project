using System.ComponentModel.DataAnnotations;

namespace WarhammerAGM.Models.Arsenal.Weapons
{
    public partial class World
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        //public string? General { get; set; }
        //public string? WildFeudal { get; set; }
        //public string? HiveWorlds { get; set; }
        //public string? ForgeWorlds { get; set; }
        //public string? WorldsFrontiers { get; set;}
        //public string? Emptiness { get; set; }
        //public string? WarZone { get; set; }
        //public string? HolyOrdos { get; set; }
    }
}
