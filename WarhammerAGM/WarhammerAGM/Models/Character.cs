namespace WarhammerAGM.Models
{
    public partial class Character : CreatureBase
    {
        public bool OnOfCharacter { get; set; }
        public Character()
        {
            OnOfCharacter = false;
        }
    }
}
