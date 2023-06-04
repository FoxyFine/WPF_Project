using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarhammerAGM.Models.Arsenal.Availability
{
    public partial class QualityAvailability
    {
        [Key]
        public int Id { get; set; }

        public string? Quality { get; set; }

        public string? PriceMultiplier { get; set; }

        public int Availability { get; set; }
    }
}
