using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace WarhammerAGM.Models
{
    public partial class Note
    {
        public string? Title { get; set; }
        public FlowDocument? FlowDocument { get; set; }
    }
}
