using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonsOfDoom.WorldObjects
{
    public class WorldObject
    {
        public char Symbol { get; set; }
        public WorldObject(char symbol)
        {
            Symbol = symbol;
        }
    }
}
