using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsOfDoom.Items
{
    abstract class Item
    {
        public string Name { get; set; }
        public int Kcal { get; set; }
        public char Symbol { get; set; }

        public Item(string name, int kcal, char symbol)
        {
            Name = name;
            Kcal = kcal;
            Symbol = symbol;
        }
    }
}
