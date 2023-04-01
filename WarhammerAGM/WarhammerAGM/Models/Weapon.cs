using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarhammerAGM.Models
{
    public partial class Weapon
    {
        [Key]
        public int Id { get; set; }
        public string? NameWeapon { get; set; }
        public string? ClassWeapon { get; set;}
        public int DistancWeapon { get; set; }
        public string? RoFWeapon { get; set; }
        public string? DamageWeapon { get; set; }
        public string? ArmorPiercingWeapon { get; set;}
        public int ClipWeapon { get; set; }
        public string? ReloadingWeapon { get; set; }
        public string? WeaponFeatures { get; set;}
        public int WeightWeapon { get; set;}
        public int PriceWeapon { get; set;}
        public string? AvailabilityWeapon { get; set;}
    }
}
