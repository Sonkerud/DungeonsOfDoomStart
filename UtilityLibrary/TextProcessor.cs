using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DungeonsOfDoom.UtilityClasses
{
    public static class TextProcessor
    {
        public static void AnimateText(string text, int delay)
        {
                Console.Write(text);
                Thread.Sleep(delay);
        }

    }
}
