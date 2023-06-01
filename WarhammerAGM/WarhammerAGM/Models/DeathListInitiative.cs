using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WarhammerAGM.Models
{
    public partial class DeathListInitiative : INotifyPropertyChanged
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int Wounds { get; set; }
        public int СurrentWounds { get; set; }
        public int DexterityModifier { get; set; }
        public int Importancenitiative { get; set; }
        public int CreatureBaseId { get; set; }
        public CreatureBase? CreatureBase { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
