using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace WarhammerAGM.Models
{
    public partial class Initiative : ViewModelBase
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int? MinPlusSlider { get; set; }
        public int Wounds { get; set; }
        public int СurrentWounds { get => Get<int>(); set => Set(value); }
        public int? DexterityModifier { get; set; }
        public int? Importancenitiative { get; set; }

        public int BestiaryCreatureId { get; set; }
        public BestiaryCreature BestiaryCreature { get; set; }
        public Initiative()
        {
            MinPlusSlider = 0;
        }
    }
}
