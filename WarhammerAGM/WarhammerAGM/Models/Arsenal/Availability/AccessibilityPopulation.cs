using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarhammerAGM.Models.Arsenal.Availability
{
    public partial class AccessibilityPopulation
    {
        [Key]
        public int Id { get; set; }

        public string? Availability { get; set; }
        public string? Less1000 { get; set; }

        public string? Less10000 { get; set; }

        public int Less100000 { get; set; }
        public int More100000 { get; set; }
    }
}
