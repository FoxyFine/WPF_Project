using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarhammerAGM.Models.Arsenal.Money
{
    public partial class IncomeAndSocialClass
    {
        [Key]
        public int Id { get; set; }
        public string? ClassCharacter { get; set; }

        public string? ThronesMonth { get; set; }

        public string? RankGrowth { get; set; }
        public string? Description { get; set; }
    }
}
