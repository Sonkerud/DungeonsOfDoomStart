﻿using DungeonsOfDoom.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsOfDoom.Characters
{
    class Monster : Character
    {
        public string Type { get; set; }
        public bool IsDead { get { return Health < 0; } }
        public Monster(int hunger, int health, char symbol, string type) : base(hunger, health, symbol) 
        {
            Type = type;
        }

        public override void Attack(Character character, int dice, int input)
        {
            switch (dice)
            {
                case 1:
                    if (character.Backpack.Count()  > 0)
                    {
                        character.Backpack.RemoveRange(0, 1);
                        Health += 5;
                    }
                    break;
                case 2:
                    if (character.Backpack.Count() > 0)
                    {
                        character.Backpack.RemoveRange(0, 1);
                        Health += 5;
                    }
                    break;
                case 3:
                    character.Backpack.Add(new Beer("Sofiero",200,'Ö'));
                    break;
                default:
                    break;
            }
        } 
    }
}
