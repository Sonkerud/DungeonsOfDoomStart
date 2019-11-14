using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityLibrary
{
    public static class SetColor
    {
        public static void SetTextColor(char c)
        {
            switch (c)
            {
                case '#':  
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case '@':
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case '%':
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case 'M':
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 'B':
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                default:
                    Console.ForegroundColor =  ConsoleColor.White;
                    break;
            }
           
        }
    }
}
