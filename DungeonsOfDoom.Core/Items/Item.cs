using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsOfDoom.Items
{
        public abstract class Item: IBagable
    {
        public string Name { get; set; }
        public int Kcal { get; set; }
        public char Symbol { get; set; }
        public int Eaten { get; set; }

        public Item(string name, int kcal, char symbol, int eaten)
        {
            Name = name;
            Kcal = kcal;
            Symbol = symbol;
            Eaten = eaten;
        }
    }
}
