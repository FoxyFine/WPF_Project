using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarhammerAGM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace WarhammerAGM.Models
    {
        public partial class RollCube
        {
            public int D_Cube { get; set; }
            public string? PlusMinus { get; set; }
            public int CubesNumber { get; set; }
            public int NumberAdditionSubtraction { get; set; }
            public RollCube() {
                CubesNumber = 1;
                D_Cube = 100;
                NumberAdditionSubtraction = 0;
                PlusMinus = "+";
            }
        }
    }

}
