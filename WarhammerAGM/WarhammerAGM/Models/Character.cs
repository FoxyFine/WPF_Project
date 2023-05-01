using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarhammerAGM.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public partial class Character
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int? Melee { get; set; }

        public int? Ballistics { get; set; }

        public int? Power { get; set; }

        public int? Endurance { get; set; }

        public int? Dexterity { get; set; }

        public int? Intelligence { get; set; }

        public int? Perception { get; set; }

        public int? Willpower { get; set; }

        public int? Partnership { get; set; }

        public int? Wounds { get; set; }

        public string? Speed { get; set; }

        public string? Skills { get; set; }

        public string? Talents { get; set; }

        public string? Armor { get; set; }

        public string? Weapons { get; set; }

        public string? Equipment { get; set; }

        public string? AdditionalFeatures { get; set; }

        public string? Features { get; set; }
        public bool OnOfCharacter { get; set; }
        public Character()
        {
            OnOfCharacter = false;
        }
    }
}
