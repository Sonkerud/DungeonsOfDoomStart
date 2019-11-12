using DungeonsOfDoom.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonsOfDoom.Characters
{
    abstract class Character
    {
        public int Hunger { get; set; }
        public char Symbol { get; set; }
        public int Health { get; set; }
        public List<Item> Backpack { get; set; }

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
