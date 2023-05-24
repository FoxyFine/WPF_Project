using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using ViewModels;

namespace WarhammerAGM.Models
{
    public partial class TemporaryInitiative : INotifyPropertyChanged
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int Wounds { get; set; }
        public int СurrentWounds { get; set; }
        public int DexterityModifier { get; set; }

        public int _importancenitiative;
        public int Importancenitiative 
        {
            get { return _importancenitiative; }
            set
            {
                _importancenitiative = value;
                OnPropertyChanged("Importancenitiative");
            }
        }
        public int CreatureBaseId { get; set; }
        public CreatureBase? CreatureBase { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
