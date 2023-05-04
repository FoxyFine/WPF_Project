using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarhammerAGM.Models
{
    public partial class Initiative
    {
        [Key]
        public int Id { get; set; }
        public string? Name {get; set; }
        public string? Type { get; set; }
        public int? MinPlusSlider { get; set; }
        public int? Wounds { get; set; }
        public int? СurrentWounds { get; set; }
        public int? DexterityModifier { get; set; }
        public int? Importancenitiative { get; set; }
        public Initiative() {
            MinPlusSlider = 0;
        } 
    }
}
