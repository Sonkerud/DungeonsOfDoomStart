using DungeonsOfDoom.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsOfDoom.Characters
{
    class Player : Character
    {
        
        public int X { get; set; }
        public int Y { get; set; }
        public bool StarvedToDeath { get { return Hunger < 100; } }


        public Player(int hunger, int health, char symbol, int x, int y ) : base(hunger, health, symbol)
        {
            X = x;
            Y = y;
            Backpack = new List<ICadaver>();
        }

        public override void Attack(Character opponent, int dice, int input)
        {
            if (dice != input)
            {
                opponent.Health -= 20;
                if (opponent.Health < 1)
                {
                    Backpack.Add(ConsoleGame.world[X, Y].Monster);
                    ConsoleGame.world[X,Y].Monster = null;
                }
            }
            else if (input == dice)
            {
                Hunger += 10;
                opponent.Health += 10;
            }
        }

        public override void EatItem()
        {
            if (Backpack.Count() != 0)
            {
                var item = Backpack.First();

                if (Hunger >= 5)
                {
                   
                        Hunger -= item.Eaten;
               
                    Backpack.RemoveAt(0);
                }
            }
        }

        public void AddItem(Room[,] world)
        {
            if (world[X,Y].Item != null)
            {
                Backpack.Add((world[X,Y].Item));
                world[X,Y].Item = null;
            }
        }
    }
}
