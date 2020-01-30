using DungeonsOfDoom.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsOfDoom.Characters
{
    public delegate void WhenAttackEventHandler(object source, EventArgs args);

    public class Monster : Character
    {
        public event WhenAttackEventHandler AttackEvent;
        public virtual void OnAttackEvent()
        {
            if (AttackEvent != null)
            {
                AttackEvent(this, EventArgs.Empty);
            }
        }

        public bool IsDead { get { return Health == 0; } }
        public Monster(int hunger, int health, char symbol, string name) : base(hunger, health, symbol) 
        {
            Name = name;
        }

        public override void Attack(Character character, int dice, int input, Room[,] world)
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
                    character.Backpack.Add(new Beer("Sofiero",200,'Ö', 5));
                    break;
                default:
                    break;
            }
        } 
    }
}
