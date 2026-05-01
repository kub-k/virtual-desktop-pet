
using System.Drawing;

namespace VirtualDesktopPet.Models
{
    public class PetSpriteSet
    {
        public Image[] WalkRight { get; set; }
        public Image[] WalkLeft { get; set; }
        public Image[] WalkUp { get; set; }
        public Image[] WalkDown { get; set; }

        public Image[] Sit { get; set; }
        public Image[] Sleep { get; set; }
    }
}
