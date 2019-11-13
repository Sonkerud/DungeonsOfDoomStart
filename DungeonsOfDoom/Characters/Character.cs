using DungeonsOfDoom.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonsOfDoom.Characters
{
        public abstract class Character : IBagable
    {
        public char Symbol { get; set; }
        private int hunger;
        public int Hunger
        {
            get
            {
                if (hunger < 0)
                {
                    return 0;
                }
                return hunger; 
            }
            set { hunger = value; }
        }

        private int health;
        public int Health
        {
            get 
            {
                if (health < 0)
                {
                    return 0;
                } 
                return health; 
            }

            set { health = value; }
        }
        public string Name { get; set; }
        public int Eaten { get; set; }
        public List<IBagable> Backpack { get; set; }

        public Character(int hunger, int health, char symbol)
        {
            Hunger = hunger;
            Health = health;
            Symbol = symbol;
        }

        public abstract void Attack(Character character, int dice, int input);
        public virtual void EatItem() { }
    }
}
