using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarhammerAGM.Models.Arsenal.Availability
{
    public partial class AvailabilityTime
    {
        [Key]
        public int Id { get; set; }

        public string? Population { get; set; }
        public string? Rarity { get; set; }

        public string? Rare { get; set; }

        public int Meager { get; set; }
        public int Average { get; set; }
        public int Common { get; set; }
        public int Plentiful { get; set; }
        public int Abundant { get; set; }
    }
}
