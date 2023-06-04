using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarhammerAGM.Models.Arsenal.Weapons
{
    public partial class Ammunition
    {

        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? PriceQuantity { get; set; }
        public string? Availability { get; set; }
    }
}
