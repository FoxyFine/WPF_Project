using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarhammerAGM.Models.Arsenal.Availability
{
    public partial class AccessibilityEffect
    {
        [Key]
        public int Id { get; set; }

        public string? Availability { get; set; }

        public string? Complexity { get; set; }

        public int Modifier { get; set; }
    }
}
