using System;

namespace MaskCrawler.Utils
{
    public class PrintUtil
    {
        public static void DW(string text, ConsoleColor foreground = ConsoleColor.Green, ConsoleColor background = ConsoleColor.Black)
        {
            W(string.Format("{0} {1}", DateTime.Now.ToString("yyyyy-MM-dd hh:mm:ss:fff"), text), foreground, background);
        }

        public static void DWL(string text, ConsoleColor foreground = ConsoleColor.Green, ConsoleColor background = ConsoleColor.Black)
        {
            WL(string.Format("{0} {1}", DateTime.Now.ToString("yyyyy-MM-dd hh:mm:ss:fff"), text), foreground, background);
        }

        public static void W(string text, ConsoleColor foreground = ConsoleColor.Green, ConsoleColor background = ConsoleColor.Black)
        {
            Write(text, foreground, background);
        }

        public static void WL(string text, ConsoleColor foreground = ConsoleColor.Green, ConsoleColor background = ConsoleColor.Black)
        {
            WriteLine(text, foreground, background);
        }

        public static void Write(string text, ConsoleColor foreground = ConsoleColor.Green, ConsoleColor background = ConsoleColor.Black)
        {
            var ftemp = Console.ForegroundColor;
            var btemp = Console.BackgroundColor;
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Write(text);
            Console.ForegroundColor = ftemp;
            Console.BackgroundColor = btemp;
        }

        public static void WriteLine(string text, ConsoleColor foreground = ConsoleColor.Green, ConsoleColor background = ConsoleColor.Black)
        {
            var ftemp = Console.ForegroundColor;
            var btemp = Console.BackgroundColor;
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.WriteLine(text);
            Console.ForegroundColor = ftemp;
            Console.BackgroundColor = btemp;
        }
    }
}
