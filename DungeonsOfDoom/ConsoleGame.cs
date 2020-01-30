using DungeonsOfDoom.Characters;
using DungeonsOfDoom.Items;
using DungeonsOfDoom.UtilityClasses;
using DungeonsOfDoom.WorldObjects;
using DungeonsOfDoomLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilityLibrary;

namespace DungeonsOfDoom
{
    public delegate void EventDelegate();

    public class ConsoleGame
    {
        public event EventDelegate OnSomethingEventDel;

        public static Player player;
        public static EnemyPlayer enemyPlayer;
        public static Room[,] world; 
        public Random random = new Random();

        public void OnMonsterAttack(object source, EventArgs e)
        {

            Console.WriteLine("A monster attacked");
        }

        public void Play()
        {
            BuildGame();
            RunGame();
            EndGame();
        }

        private void BuildGame()
        {
            Console.Clear();
            CreatePlayer();
            CreateEnemyPlayer();
            CreateWorld();
            ClearSpaceForMovingWall();
            CreateWall();

            TextToDisplayLibrary.AddTextToDictionary();
            DisplayWorld();
        }
        private void RunGame()
        {
            do
            {
                    for (int i = 0; i < world.GetLength(0); i++)
                    {
                       
                        CreateSpaceInMovingWall(i);
                    if (player.StarvedToDeath)
                    {
                        GameOver();
                    }
                        DisplayStats();
                        BuildTextDisplayBox();

                        MoveEnemyPlayer(enemyPlayer, player, i);
                        var key = AskForMovement(player);
                        // AskForMovement(enemyPlayer,key);
                        // DisplayPartOfWorld(player,player.X, player.Y);
                        // DisplayPartOfWorld(enemyPlayer,enemyPlayer.X, enemyPlayer.Y);

                        AddItem();
                        FightWithMonster(player.X, player.Y);
                        DisplayStats();
                        Thread.Sleep(100);
                    }
            }
            while (!player.StarvedToDeath || (CountRemainingMonsters() == 0 && CountRemainingBosses() == 0));
        }
        private void EndGame()
        {
            if (player.StarvedToDeath)
            {
                GameOver();
            }
            else if (CountRemainingMonsters() == 0 && CountRemainingBosses() == 0)
            {
                ShowWinningScreen();
            }
        }

        private void CreateWall()
        {
            for (int y = 7; y < 8; y++)
            {
                for (int x = 0; x < world.GetLength(0); x++)
                {
                    world[x, y].WorldObject = new WallObject('#');
                }
            }
        }
        private void CreateSpaceInMovingWall(int x)
        {
            if (x-2 != player.X)
            {
                if (x > 2)
                {
                    world[x - 3, 7].WorldObject = new WallObject('#');
                    world[x - 2, 7].WorldObject = new WallObject('#');
                    world[x - 1, 7].WorldObject = new WallObject('#');
                    world[x, 7].WorldObject = new WallObject('#');
                }

                if (x < world.GetLength(0) - 2)
                {
                    world[x, 7].WorldObject = null;
                    world[x + 1, 7].WorldObject = null;
                    world[x + 2, 7].WorldObject = null;
                }
            }
            else if (x-2 == player.X && player.Y == 7)
            {
                player.Hunger = 11000;
            }
            Console.SetCursorPosition(0,7);
            for (int i = 0; i < world.GetLength(0); i++)
            {
                Room room = world[i, 7];
                if (player.X == i && player.Y == 7)
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
        }
        private void ClearSpaceForMovingWall()
        {

            for (int y = 7; y < 8; y++)
            {
                for (int x = 0; x < world.GetLength(0); x++)
                {
                    
                    if (world[x, y].Item != null)
                    {
                        world[x, y].Item = null;
                    }
                    if (world[x, y].Monster != null)
                    {
                        world[x, y].Monster = null;
                    }
                    if (world[x, y].WorldObject != null)
                    {
                        world[x, y].WorldObject = null;
                    }

                }
            }
        }

        private void CreatePlayer()
        {
            player = new Player(90, 30, '@', 1, 1);
        }
        private void CreateEnemyPlayer()
        {
            enemyPlayer = new EnemyPlayer(90, 30, '%', 38, 13);
        }
        private void BuildTextDisplayBox()
        {
            string top = " _________________________________";
            string bottom = "|_________________________________|";
            SetColor.SetTextColor('%');

            Console.SetCursorPosition(0, 16);
            Console.Write(top);
            for (int row = 17; row < 24; row++)
            {
                Console.SetCursorPosition(0, row);
                Console.Write("|");
                Console.SetCursorPosition(34, row);
                Console.Write("|");
            }
            Console.SetCursorPosition(0, 24);
            Console.Write(bottom);
            SetColor.SetTextColor('d');

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
                        world[x, y].Item = new Beer("Carlsberg", 200, 'Ö', 5);
                    else if (percentage < 15)
                        world[x, y].Item = new Pizza("Hawaii", 1200, 'O', 10);
                    else if (percentage < 20)
                        world[x, y].Item = new Pizza("Calzone", 1500, 'O', 10);
                    else if (percentage < 21)
                        world[x, y].Monster = new BossMonster();
                    else if (percentage < 30)
                        world[x, y].WorldObject = new FenceObject('|');
                }
            }

            for (int x = 0; x < 1; x++)
            {
                for (int y = 0; y < world.GetLength(1); y++)
                {
                    world[x, y].WorldObject = new WallObject('#');
                }
            }
            for (int x = world.GetLength(0)-1; x < world.GetLength(0); x++)
            {
                for (int y = 0; y < world.GetLength(1); y++)
                {
                    world[x, y].WorldObject = new WallObject('#');
                }
            }
            for (int y = 0; y < 1; y++)
            {
                for (int x = 0; x < world.GetLength(0); x++)
                {
                    world[x, y].WorldObject = new WallObject('#');
                }
            }
            for (int y = world.GetLength(1) - 1; y < world.GetLength(1); y++)
            {
                for (int x = 0; x < world.GetLength(0); x++)
                {
                    world[x, y].WorldObject = new WallObject('#');
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
                    {
                        SetColor.SetTextColor(player.Symbol);
                        Console.Write(player.Symbol);
                        SetColor.SetTextColor('d');

                    }
                    else if (enemyPlayer.X == x && enemyPlayer.Y == y) 
                    { 
                        Console.Write(enemyPlayer.Symbol);
                    }
                    else if (room.WorldObject != null)
                    {
                        SetColor.SetTextColor(room.WorldObject.Symbol);
                        Console.Write(room.WorldObject.Symbol);
                        SetColor.SetTextColor('d');

                    }

                    else if (room.Monster != null)
                    {
                        SetColor.SetTextColor(room.Monster.Symbol);
                        Console.Write(room.Monster.Symbol);
                        SetColor.SetTextColor('d');

                    }
                    else if (room.Item != null)
                    {
                        SetColor.SetTextColor(room.Item.Symbol);
                        Console.Write(room.Item.Symbol);
                        SetColor.SetTextColor('d');

                    }
                    else
                        Console.Write(" ");
                }
                Console.WriteLine();
            }

        }
        private void DisplayPartOfWorldEarlier(Player character,int positionX, int positionY)
        {
            for (int y = positionY-1; y < positionY+2; y++)
            {
                for (int x = positionX-1; x < positionX+2; x++)
                {
                    Console.SetCursorPosition(x,y);
                    Room room = world[x, y];

                    if (character.X == x && character.Y == y)
                    {
                       // SetColor.SetTextColor(character.Symbol);
                        Console.Write(character.Symbol);
                    }
                    else if (room.WorldObject != null)
                    {
                       // SetColor.SetTextColor(room.WorldObject.Symbol);
                        Console.Write(room.WorldObject.Symbol);
                    }
                    else if (room.Monster != null)
                    {
                        //SetColor.SetTextColor(room.Monster.Symbol);
                        Console.Write(room.Monster.Symbol);
                    }
                    else if (room.Item != null)
                    {
                       // SetColor.SetTextColor(room.Item.Symbol);
                        Console.Write(room.Item.Symbol);
                    }
                    else
                        // Console.ForegroundColor = ConsoleColor.Black;    
                         Console.Write(" ");
                }
                Console.WriteLine();
            }

        }
        private void DisplayPartOfWorld(Player character, int x, int y)
        {
           
                    Console.SetCursorPosition(x, y);
                    Room room = world[x, y];

                    if (character.X == x && character.Y == y)
                    {
                        SetColor.SetTextColor(character.Symbol);
                        Console.Write(character.Symbol);
                    }
                    else if (room.WorldObject != null)
                    {
                        //SetColor.SetTextColor(room.WorldObject.Symbol);
                        Console.Write(room.WorldObject.Symbol);
                    }
                    else if (room.Monster != null)
                    {
                        //SetColor.SetTextColor(room.Monster.Symbol);
                        Console.Write(room.Monster.Symbol);
                    }
                    else if (room.Item != null)
                    {
                        // SetColor.SetTextColor(room.Item.Symbol);
                        Console.Write(room.Item.Symbol);
                    }
                    else
                        // Console.ForegroundColor = ConsoleColor.Black;    
                        Console.Write(" ");

        }

        private void DisplayStats()
        {
            SetColor.SetTextColor('#');

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
            if (enemyPlayer.Backpack != null)
            {
                Console.SetCursorPosition(1, 23);
                Console.WriteLine($"Snodda items: {enemyPlayer.Backpack.Count()}  ");
            }

            Console.SetCursorPosition(1, 20);
            Console.WriteLine($"Monster kvar: {CountRemainingMonsters()}  ");
            Console.SetCursorPosition(1, 21);
            Console.WriteLine($"Bossar kvar: {CountRemainingBosses()}  ");
            SetColor.SetTextColor('d');

        }
        private ConsoleKeyInfo AskForMovement(Player player)
        {
            ConsoleKeyInfo testKey = new ConsoleKeyInfo();
            if (Console.KeyAvailable)
            {
                testKey = Console.ReadKey(true);
                switch (testKey.Key)
                {
                    case ConsoleKey.RightArrow:
                        MoveRight(player);
                        break;
                    case ConsoleKey.LeftArrow:
                        MoveLeft(player);
                        break;
                    case ConsoleKey.UpArrow:
                        MoveUp(player);
                        break;
                    case ConsoleKey.DownArrow:
                        MoveDown(player);
                        break;
                    case ConsoleKey.U:
                        player.EatItem();
                        break;
                    default:
                        break;
                }
                return testKey; 
            }
            return testKey;
        }
        private void AskForMovement(Player player, ConsoleKeyInfo testKey)
        {

                switch (testKey.Key)
                {
                    case ConsoleKey.RightArrow:
                        MoveRight(player);
                        break;
                    case ConsoleKey.LeftArrow:
                        MoveLeft(player);
                        break;
                    case ConsoleKey.UpArrow:
                        MoveUp(player);
                        break;
                    case ConsoleKey.DownArrow:
                        MoveDown(player);
                        break;
                    case ConsoleKey.U:
                        player.EatItem();
                        break;
                    default:
                        break;
            }
        }

        private void MoveEnemyPlayer(Player enemy, Player player, int i)
        {
            if (i%3==0)
            {
                int[] directionList = new int[10];
                // Om X är positivt är enemy höger om player
                int movementDirectionX =  enemy.X - player.X;
                // Om Y är positivt är enemy nedanför player
                int movementDirectionY =  enemy.Y - player.Y;

                if (movementDirectionX >= 0)
                {
                    directionList[0] = 2;
                    directionList[1] = 2;
                    directionList[2] = 2;
                    directionList[3] = 2;
                    directionList[8] = 1;
                }
                else
                {
                    directionList[0] = 1;
                    directionList[1] = 1;
                    directionList[2] = 1;
                    directionList[3] = 1;
                    directionList[8] = 2;
                }

                if (movementDirectionY >= 0)
                {
                    directionList[4] = 3;
                    directionList[5] = 3;
                    directionList[6] = 3;
                    directionList[7] = 3;
                    directionList[9] = 4;
                }
                else
                {
                    directionList[4] = 4;
                    directionList[5] = 4;
                    directionList[6] = 4;
                    directionList[7] = 4;
                    directionList[9] = 3;
                }

                Random random = new Random();
                int dice = random.Next(0, 9);


                switch (directionList[dice])
                {
                    case 1:
                        MoveRight(enemy);
                        break;
                    case 2:
                        MoveLeft(enemy);
                        break;
                    case 3:
                        MoveUp(enemy);
                        break;
                    case 4:
                        MoveDown(enemy);
                        break;
                    default:
                        break;
                }
            }   
        }
            
        
        private void MoveUp(Player player)
        {

            if (world[player.X, player.Y - 1].WorldObject == null)
            {
                player.Y--;
                DisplayPartOfWorld(player, player.X, player.Y);
                DisplayPartOfWorld(player, player.X, player.Y+1);
            }
        }
        private void MoveDown(Player player)
        {
            if (world[player.X, player.Y + 1].WorldObject == null)
            {
                player.Y++;
                DisplayPartOfWorld(player, player.X, player.Y);
                DisplayPartOfWorld(player, player.X , player.Y-1);
            }
        }
        private void MoveLeft(Player player)
        {
            if (world[player.X - 1, player.Y].WorldObject == null)
            {
                player.X--;
                DisplayPartOfWorld(player, player.X, player.Y);
                DisplayPartOfWorld(player, player.X + 1, player.Y);
            }
        }
        private void MoveRight(Player player)
        {
            if (world[player.X + 1, player.Y].WorldObject == null)
            {
                player.X++;
                DisplayPartOfWorld(player, player.X, player.Y);
                DisplayPartOfWorld(player, player.X-1, player.Y);
            }
        }
        private void AddItem()
        {
            player.AddItem(world);
            enemyPlayer.AddItem(world);
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
                
                Console.SetCursorPosition(SetCursorConstant.SETCURSORFORFIGHTINGINFOX, SetCursorConstant.SETCURSORFORFIGHTINGINFOY);
                TextProcessor.AnimateText(TextToDisplayLibrary.textToDisplayList[0],500);
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
                            Console.SetCursorPosition(SetCursorConstant.SETCURSORFORFIGHTINGINFOX, SetCursorConstant.SETCURSORFORFIGHTINGINFOY + 1);
                            Console.WriteLine("Ange 1 till 3");
                            Console.SetCursorPosition(108, 3);
                        }
                    }
                    catch (Exception)
                    {
                        Console.SetCursorPosition(SetCursorConstant.SETCURSORFORFIGHTINGINFOX, SetCursorConstant.SETCURSORFORFIGHTINGINFOY + 1);
                        Console.WriteLine("Ange 1 till 3");
                        Console.SetCursorPosition(108, 3);
                    }
                }
                Console.SetCursorPosition(SetCursorConstant.SETCURSORFORFIGHTINGINFOX, SetCursorConstant.SETCURSORFORFIGHTINGINFOY+2);

                //Player attacks
                player.Attack(monster, dice, input, world);
                //Console information for player.attacks():
                if (dice != input)
                {
                    Console.SetCursorPosition(SetCursorConstant.SETCURSORFORFIGHTINGINFOX, SetCursorConstant.SETCURSORFORFIGHTINGINFOY + 2);
                    TextProcessor.AnimateText($"Du smaskade till {monster.Name} med en Hawaii!  ",0);
                    
                
                    if (monster.IsDead)
                    {
                        Console.SetCursorPosition(SetCursorConstant.SETCURSORFORFIGHTINGINFOX, SetCursorConstant.SETCURSORFORFIGHTINGINFOY);
                        TextProcessor.AnimateText($"Snyggt pizzat! Du besegrade {monster.Name}!                        ", 2000);

                        Console.WriteLine();
                        
                        //Clear text-display-field
                        for (int i = 4; i < 16; i++)
                        {
                            Console.SetCursorPosition(54, i);
                            Console.WriteLine("                                                               ");
                        }
                    }
                    if (monster != null)
                    {
                        Console.SetCursorPosition(SetCursorConstant.SETCURSORFORFIGHTINGINFOX, SetCursorConstant.SETCURSORFORFIGHTINGINFOY + 3);
                        TextProcessor.AnimateText($"{monster.Name} health: {monster.Health}",500); 
                    }
                }
                else if (input == dice)
                {
                    Console.SetCursorPosition(SetCursorConstant.SETCURSORFORFIGHTINGINFOX, SetCursorConstant.SETCURSORFORFIGHTINGINFOY + 2);
                    Console.WriteLine($"Aj aj! {monster.Name} högg dig i vaden!                ");
                    Console.SetCursorPosition(SetCursorConstant.SETCURSORFORFIGHTINGINFOX, SetCursorConstant.SETCURSORFORFIGHTINGINFOY + 3);
                    Console.WriteLine($"{monster.Name} health: {monster.Health}");
                }
            }

            //If monster still exists, monster attacks:
            if (room.Monster != null)
            {
                Console.SetCursorPosition(55, 9);
                TextProcessor.AnimateText($"{monster.Name} kontrar! Fly din dåre!              ",2000);

                if (!Console.KeyAvailable)
                {
                    room.Monster.Attack(player, dice, input, world);
                    switch (dice)
                    {
                        case 1:
                            if (player.Backpack.Count() > 0)
                            {
                                Console.SetCursorPosition(55, 11);
                                TextProcessor.AnimateText($"{monster.Name} fick tag på dig och stal en pizza eller nått! Attans!                 ", 1000);
                            }
                            else
                            {
                                Console.SetCursorPosition(55, 11);
                                TextProcessor.AnimateText($"{monster.Name} fick tag på dig men du hade en tom backpack!             ", 1000);
                            }
                            break;
                        case 2:
                            if (player.Backpack.Count() > 0)
                            {
                                Console.SetCursorPosition(55, 11);
                                TextProcessor.AnimateText($"{monster.Name} fick tag på dig och stal en pizza eller nått! Attans!                 ", 1000);
                            }
                            else
                            {
                                Console.SetCursorPosition(55, 11);
                                Console.WriteLine($"{monster.Name} fick tag på dig men du hade en tom backpack!   ");
                            }
                            break;
                        case 3:
                            Console.SetCursorPosition(55, 11);
                            TextProcessor.AnimateText($"Smidig som en iller undvek du {monster.Name}! Du får en öl!            ", 0);
                            break;
                        default:
                            break;
                    }
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
            Console.WriteLine("You starved to death dude. Wtf. So much Pizza.");
            Console.WriteLine("How is it possible? Well well. Try again.");
            Console.ReadKey();
            Play();
        }
        private void ShowWinningScreen()
        {
            Console.Clear();

            Console.WriteLine(" __      __.__                           ._.");
            Console.WriteLine("/  \\    /  \\__| ____   ____   ___________| |");
            Console.WriteLine("\\   \\/\\/   /  |/    \\ /    \\_/ __ \\_  __ \\ |");
            Console.WriteLine(" \\        /|  |   |  \\   |  \\  ___/|  | \\/\\|");
            Console.WriteLine("  \\__/\\  / |__|___|  /___|  /\\___  >__|   __");
            Console.WriteLine("       \\/          \\/     \\/     \\/       \\/");

            Console.ReadKey();
        }
    }
}
