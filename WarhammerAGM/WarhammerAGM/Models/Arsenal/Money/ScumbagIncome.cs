using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarhammerAGM.Models.Arsenal.Money
{
    public partial class ScumbagIncome
    {
        [Key]
        public int Id { get; set; }
        public string? D10 { get; set; }

        public string? Result { get; set; }

        public string? Income { get; set; }
    }
}
