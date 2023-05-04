using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WarhammerAGM.Models
{
    public partial class ListCube : INotifyPropertyChanged
    {
        string? _namecube;
        string? _cuberesult;
        string? _cuberesulttooltip;
        public string? CubeResult
        {
            get { return _cuberesult; }
            set
            {
                _cuberesult = value;
                OnPropertyChanged("CubeResult");
            }
        }
        public string? CubeResultToolTip
        {
            get { return _cuberesulttooltip; }
            set
            {
                _cuberesulttooltip = value;
                OnPropertyChanged("CubeResultToolTip");
            }
        }
        public string? NameCube
        {
            get { return _namecube; }
            set
            {
                _namecube = value;
                OnPropertyChanged("NameCube");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
