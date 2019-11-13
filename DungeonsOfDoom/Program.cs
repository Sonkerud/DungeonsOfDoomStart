using DungeonsOfDoom.Characters;
using DungeonsOfDoom.Items;
using System;
using System.Collections.Generic;

namespace DungeonsOfDoom
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleGame game = new ConsoleGame();
            game.Play();

            //List<ISymbolInterface> symbolInterfaceList = new List<ISymbolInterface>();

            //symbolInterfaceList.Add(new Player(10,10,'G',10,10));
            //symbolInterfaceList.Add(new Beer("brun",100,'H',100));
            

        }
    }   
}
