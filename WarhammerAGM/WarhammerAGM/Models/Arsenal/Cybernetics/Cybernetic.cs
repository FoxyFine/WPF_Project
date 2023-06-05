using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarhammerAGM.Models.Arsenal.Cybernetics
{
    public partial class Cybernetic
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Price { get; set; }
        public string? Availability { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
