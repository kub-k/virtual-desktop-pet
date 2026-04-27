using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDesktopPet.Models
{
    public class PetConfig
    {
        public int MovementSpeed { get; set; }
        public int StateChangeInterval { get; set; }
        public int WalkChance { get; set; }
        public int SitChance { get; set; }
        public int SleepChance { get; set; }
        public int SelectedMonitorIndex { get; set; }
    }
}
