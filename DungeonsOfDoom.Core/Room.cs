using DungeonsOfDoom.Characters;
using DungeonsOfDoom.Items;
using DungeonsOfDoom.WorldObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsOfDoom
{
    public class Room
    {
        public Monster Monster { get; set; }
        public Item Item { get; set; }
        public WorldObject WorldObject { get; set; }
        public BossMonster BossMonster { get; set; }
        
    }
}
