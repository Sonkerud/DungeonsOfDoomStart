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
    class ConsoleGame
    {
        public static Player player;
        public static Room[,] world; 
        public Random random = new Random();

        public void Play()
        {
            Console.Clear();
            CreatePlayer();
            CreateWorld();
            DisplayWorld();
            do
            {
                DisplayStats();
                BuildTextDisplayBox();
                AskForMovement();
                DisplayPartOfWorld(player.X, player.Y);
                AddItem();
                FightWithMonster(player.X, player.Y);
                DisplayStats();
            } while (player.StarvedToDeath || (CountRemainingMonsters() == 0 && CountRemainingBosses() == 0));
            GameOver();
        }
        private void CreatePlayer()
        {
            player = new Player(90, 30, '@', 1, 1);
        }
        private void BuildTextDisplayBox()
        {
            string top = " _________________________________";
            string bottom = "|_________________________________|";

            Console.SetCursorPosition(0, 16);
            Console.Write(top);
            for (int row = 17; row < 23; row++)
            {
                Console.SetCursorPosition(0, row);
                Console.Write("|");
                Console.SetCursorPosition(34, row);
                Console.Write("|");
            }
            Console.SetCursorPosition(0, 23);
            Console.Write(bottom);
        }
        private void CreateWorld()
        {
            world = new Room[40, 15];
            for (int y = 0; y < world.GetLength(1); y++)
            {
                for (int x = 0; x < world.GetLength(0); x++)
                {
                    world[x, y] = new Room();

                    int percentage = random.Next(0, 100);
                    if (percentage < 2)
                        world[x, y].Monster = new Monster(30,30, 'M', "Monstret");
                    else if (percentage < 10)
                        world[x, y].Item = new Beer("Carlsberg", 200, 'Ö');
                    else if (percentage < 15)
                        world[x, y].Item = new Pizza("Hawaii", 1200, 'O');
                    else if (percentage < 20)
                        world[x, y].Item = new Pizza("Calzone", 1500, 'O');
                    else if (percentage < 21)
                        world[x, y].Monster = new BossMonster();
                    else if (percentage < 30)
                        world[x, y].WorldObject = new FenceObject();
                }
            }

            for (int x = 0; x < 1; x++)
            {
                for (int y = 0; y < world.GetLength(1); y++)
                {
                    world[x, y].WorldObject = new WallObject();
                }
            }
            for (int x = world.GetLength(0)-1; x < world.GetLength(0); x++)
            {
                for (int y = 0; y < world.GetLength(1); y++)
                {
                    world[x, y].WorldObject = new WallObject();
                }
            }
            for (int y = 0; y < 1; y++)
            {
                for (int x = 0; x < world.GetLength(0); x++)
                {
                    world[x, y].WorldObject = new WallObject();
                }
            }
            for (int y = world.GetLength(1) - 1; y < world.GetLength(1); y++)
            {
                for (int x = 0; x < world.GetLength(0); x++)
                {
                    world[x, y].WorldObject = new WallObject();
                }
            }

        }

        private void DisplayWorld()
        {
            for (int y = 0; y < world.GetLength(1); y++)
            {
                for (int x = 0; x < world.GetLength(0); x++)
                {
                    Room room = world[x, y];
                    if (player.X == x && player.Y == y)
                        Console.Write(player.Symbol);
                    else if (room.WorldObject != null)
                        Console.Write(room.WorldObject.Symbol);
                    else if (room.Monster != null)
                        Console.Write(room.Monster.Symbol);
                    else if (room.Item != null)
                        Console.Write(room.Item.Symbol);
                    else
                        Console.Write(" ");
                }
                Console.WriteLine();
            }

        }
        private void DisplayPartOfWorld(int positionX, int positionY)
        {
            for (int y = positionY-1; y < positionY+2; y++)
            {
                for (int x = positionX-1; x < positionX+2; x++)
                {
                    Console.SetCursorPosition(x,y);
                    Room room = world[x, y];
                    if (player.X == x && player.Y == y)
                        Console.Write(player.Symbol);
                    else if (room.WorldObject != null)
                        Console.Write(room.WorldObject.Symbol);
                    else if (room.Monster != null)
                        Console.Write(room.Monster.Symbol);
                    else if (room.Item != null)
                        Console.Write(room.Item.Symbol);

                    else
                        Console.Write(" ");
                }
                Console.WriteLine();
            }

        }
        private void DisplayStats()
        {
            Console.SetCursorPosition(1, 17);
            Console.WriteLine($"Hunger: {player.Hunger}   ");

            if (player.Backpack != null)
            {
                Console.SetCursorPosition(1, 18);
                Console.WriteLine($"Antal item backpack: {player.Backpack.Count()}  ");
                Console.SetCursorPosition(1, 19);
                Console.WriteLine("                                   ");
                Console.SetCursorPosition(1, 19);

                foreach (var item in player.Backpack.Take(3))
                {
                    Console.Write($"{item.Name} ");
                }
            }
            Console.SetCursorPosition(1, 20);
            Console.WriteLine($"Monster kvar: {CountRemainingMonsters()}  ");
            Console.SetCursorPosition(1, 21);
            Console.WriteLine($"Bossar kvar: {CountRemainingBosses()}  ");
        }
        private void AskForMovement()
        {

            bool isValidKey = true;

            ConsoleKeyInfo keyInfo = Console.ReadKey();
            switch (keyInfo.Key)
            {
                case ConsoleKey.RightArrow:
                    MoveRight();
                    break;
                case ConsoleKey.LeftArrow:
                    MoveLeft();
                    break;
                case ConsoleKey.UpArrow:
                    MoveUp();
                    break;
                case ConsoleKey.DownArrow:
                    MoveDown();
                    break;
                case ConsoleKey.U: player.EatItem(); 
                    break;
                default:
                    break;
            }
        }

        private void MoveUp()
        {

            if (world[player.X, player.Y - 1].WorldObject == null)
            {
                player.Y--;
            }
        }
        private void MoveDown()
        {
            if (world[player.X, player.Y + 1].WorldObject == null)
            {
                player.Y++;
            }
        }
        private void MoveLeft()
        {
            if (world[player.X - 1, player.Y].WorldObject == null)
            {
                player.X--;
            }
        }
        private void MoveRight()
        {
            if (world[player.X + 1, player.Y].WorldObject == null)
            {
                player.X++;
            }
        }
        private void AddItem()
        {
            player.AddItem(world);
        }

        private void FightWithMonster(int x, int y)
        {
            Monster monster = world[x, y].Monster;
            Room room = world[x, y];
            //Clear fighting-text-field
            for (int i = 2; i < 13; i++)
            {
                Console.SetCursorPosition(52, i);
                Console.WriteLine("                                                                    ");
            }
            int dice = random.Next(1, 4);
            int input = 1;

            if (world[x,y].Monster != null)
            {
                
                Console.SetCursorPosition(55, 3);
                Console.WriteLine("Slåss med monstret! Ange siffra mellan 1 och 3:     ");
                Console.SetCursorPosition(103, 3);
                
                //Control input for dice/randomnumber comparer
                bool inputBool = false;
                while (!inputBool)
                {
                    try
                    {
                        int userInput = int.Parse(Console.ReadLine());
                        if (userInput == 1 || userInput == 2 || userInput == 3)
                        {
                            input = userInput;
                            inputBool = true;
                        }
                        else
                        {
                            Console.SetCursorPosition(55, 4);
                            Console.WriteLine("Ange 1 till 3");
                            Console.SetCursorPosition(108, 3);
                        }

                    }
                    catch (Exception)
                    {
                        Console.SetCursorPosition(55, 4);
                        Console.WriteLine("Ange 1 till 3");
                        Console.SetCursorPosition(108, 3);
                    }
                }
                Console.SetCursorPosition(55, 5);

                //Player attacks
                player.Attack(monster, dice, input);
                //Console information for player.attacks():
                if (dice != input)
                {
                    Console.SetCursorPosition(55, 5);
                    Console.WriteLine($"Du smaskade till {monster.Type} med en Hawaii!  ");
                
                    if (monster.IsDead)
                    {
                        Console.SetCursorPosition(55, 3);
                        Console.WriteLine($"Snyggt pizzat! Du besegrade {monster.Type}!                        ");
                        
                        //Clear text-display-field
                        for (int i = 4; i < 16; i++)
                        {
                            Console.SetCursorPosition(54, i);
                            Console.WriteLine("                                                               ");
                        }
                    }
                    if (monster != null)
                    {
                        Console.SetCursorPosition(55, 6);
                        Console.WriteLine($"{monster.Type} health: {monster.Health}");
                    }
                }
                else if (input == dice)
                {
                    Console.SetCursorPosition(55, 5);
                    Console.WriteLine($"Aj aj! {monster.Type} högg dig i vaden!                ");
                    Console.SetCursorPosition(55, 6);
                    Console.WriteLine($"{monster.Type} health: {monster.Health}");
                }
            }

            //If monster still exists, monster attacks:
            if (room.Monster != null)
            {
                Console.SetCursorPosition(55, 9);
                Console.WriteLine($"{monster.Type} kontrar! Fly din dåre!              ");
               
                room.Monster.Attack(player, dice, input);
                //Console information for monster.attacks();
                switch (dice)
                {
                    case 1:
                        if (player.Backpack.Count() > 0)
                        {
                            Console.SetCursorPosition(55, 11);
                            Console.WriteLine($"{monster.Type} fick tag på dig och stal en pizza eller nått! Attans!                 ");
                        }
                        else
                        {
                            Console.SetCursorPosition(55, 11);
                            Console.WriteLine($"{monster.Type} fick tag på dig men du hade en tom backpack!             ");
                        }
                        break;
                    case 2:
                        if (player.Backpack.Count() > 0)
                        {
                            Console.SetCursorPosition(55, 11);
                            Console.WriteLine($"{monster.Type} fick tag i dig och stal en pizza eller nått! Attans!                     ");
                        }
                        else
                        {
                            Console.SetCursorPosition(55, 11);
                            Console.WriteLine($"{monster.Type} fick tag på dig men du hade en tom backpack!   ");
                        }
                        break;
                    case 3:
                            Console.SetCursorPosition(55, 11);
                            Console.WriteLine($"Smidig som en iller undvek du {monster.Type}! Du får en öl!            ");
                        break;
                    default:
                        break;
                }
            }
        }
        private int CountRemainingMonsters()
        {
            int monsterCount = 0;
            for (int i = 0; i < world.GetLength(0); i++)
            {
                for (int y = 0; y < world.GetLength(1); y++)
                {
                    if (world[i,y].Monster != null && world[i, y].Monster is Monster)
                    {
                        monsterCount += 1;
                    }
                }
            }
            return (monsterCount);
        }
        private int CountRemainingBosses()
        {
            
            int bossCount = 0;
            for (int i = 0; i < world.GetLength(0); i++)
            {
                for (int y = 0; y < world.GetLength(1); y++)
                {
                    if (world[i, y].Monster != null && world[i, y].Monster is BossMonster)
                    {
                       bossCount += 1;
                    }
                }
            }
            return (bossCount);
        }

        private void GameOver()
        {
            Console.Clear();
            Console.WriteLine("Game over...");
            Console.ReadKey();
            Play();
        }
    }
}
