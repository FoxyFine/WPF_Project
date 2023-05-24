﻿using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace WarhammerAGM.Models
{
    //[Index(nameof(Name), IsUnique = true)]
    [Index("Name", IsUnique = true)]
    public partial class CreatureBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int Melee { get; set; }

        public int Ballistics { get; set; }

        public int Power { get; set; }

        public int Endurance { get; set; }

        public int Dexterity { get; set; }

        public int Intelligence { get; set; }

        public int Perception { get; set; }

        public int Willpower { get; set; }

        public int Partnership { get; set; }

        public int Wounds { get; set; }

        public string? Speed { get; set; }

        public string? Skills { get; set; }

        public string? Talents { get; set; }

        public string? Armor { get; set; }

        public string? Weapons { get; set; }

        public string? Equipment { get; set; }

        public string? AdditionalFeatures { get; set; }

        public string? Features { get; set; }
        public string? Discriminator { get; set; }
        public CreatureBase()
        {
            Melee = 0;
            Ballistics = 0;
            Power = 0;
            Endurance = 0;
            Dexterity = 0;
            Intelligence = 0;
            Perception = 0;
            Willpower = 0;
            Partnership = 0;
            Wounds = 0;
        }
    }
}
