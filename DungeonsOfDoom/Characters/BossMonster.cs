using DungeonsOfDoom.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonsOfDoom.Characters
{
    class BossMonster : Monster
    {
        Random random = new Random();

        public BossMonster() : base(30, 100, 'B', "Bossmonstret")
        {

        }

        public override void Attack(Character character, int dice, int input)
        {
            dice = random.Next(1, 3);
            Console.SetCursorPosition(55, 9);
            Console.WriteLine("Bossen är ute efter dig!                            ");

            switch (dice)
            {
                case 1:

                    Console.SetCursorPosition(55, 11);
                    Console.WriteLine("Bossen fick tag på dig och gjorde dig massa hungrig!                 ");
                    Health += 5;
                    character.Hunger += 30;

                    break;
                case 2:
                    if (Health >= 50)
                    {
                        Health -= 50;
                        Console.SetCursorPosition(55, 11);
                        Console.WriteLine("Oj vilka moves! Bossen halkade! -50 health!                ");
                    }
                    else if (Health <50)
                    {
                        Health = 0;
                        Console.SetCursorPosition(55, 11);
                        Console.WriteLine("Bossen dog !                            ");
                        ConsoleGame.world[ConsoleGame.player.X, ConsoleGame.player.Y].Monster = null;
                    }
                      break;
                default:
                    break;
            }
        }
    }
}
