using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarhammerAGM.Models
{
    public partial class Character : BestiaryCreature
    {
        public bool OnOfCharacter { get; set; }

        /*public int InitiativeId { get; set; }
        public Initiative Initiative { get; set; }*/
        public Character()
        {
            OnOfCharacter = false;
        }
    }
}
