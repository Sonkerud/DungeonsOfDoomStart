using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using DungeonsOfDoom.Characters;

namespace DungeonsOfDoomLibrary
{
    public static class TextToDisplayLibrary
    {
        public static List<string> textToDisplayList = new List<string>();

        public static void AnimateText(string text, int delay)
        {
                Console.WriteLine(text);
                Thread.Sleep(delay);
        }
        public static void AnimateText(string text, int delay, Character character)
        {
            Console.WriteLine(text);
            Thread.Sleep(delay);
        }

        public static void AddTextToDictionary()
        {
            textToDisplayList.Add("Slåss med monstret! Ange siffra mellan 1 och 3:     ");
        }
    }
}
