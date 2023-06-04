using System.ComponentModel.DataAnnotations;

namespace WarhammerAGM.Models.Arsenal.Weapons
{
    public partial class RangedWeapon
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? MeleeWeaponClass { get; set; }
        public string? Distance { get; set; }
        public string? RateOfFire { get; set; }
        public string? Damage { get; set; }
        public string? ArmorPiercing { get; set; }
        public string? Clip { get; set; }
        public string? Recharge { get; set; }
        public string? Features { get; set; }
        public string? Weight { get; set; }
        public string? Price { get; set; }
        public string? Availability { get; set; }
        public string? World { get; set; }
        public string? Type { get; set; }
    }
}
