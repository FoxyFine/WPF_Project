using System.ComponentModel.DataAnnotations;

namespace WarhammerAGM.Models.Arsenal
{
    public partial class Armor
    {

        [Key]
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? Name { get; set; }
        public string? ProtectionZones { get; set; }
        public string? Protection { get; set; }
        public string? Weight { get; set; }
        public string? Price { get; set; }
        public string? Availability { get; set; }
        public string? World { get; set; }
    }
}
