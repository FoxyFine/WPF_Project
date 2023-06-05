using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarhammerAGM.Models.CriticalDamage
{
    public partial class CriticalDamage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CriticalDamageUnits { get; set; }
        [Required]
        public string Effect { get; set; }
        [Required]
        public string BodyPart { get; set; }
        [Required]
        public string TypeDamage { get; set; }
    }
}
