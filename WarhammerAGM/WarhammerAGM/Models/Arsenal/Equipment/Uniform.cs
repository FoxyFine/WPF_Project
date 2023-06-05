using System.ComponentModel.DataAnnotations;

namespace WarhammerAGM.Models.Arsenal.Equipment
{
    public partial class Uniform
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Weight { get; set; }
        public string? Price { get; set; }
        public string? Availability { get; set; }
        public string? World { get; set; }
    }
}
