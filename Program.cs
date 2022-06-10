using System;
using MyCozmoLib;

namespace MyCozmo
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Cozmo.GetHelp());
            var cozmo = new Cozmo();

            while(true)
            {
                Console.WriteLine("Type your command here:\n");
                var cmd = Console.ReadLine();
                var msg = String.Empty;
                var doContinue = cozmo.ProcessCommand(cmd, out msg);

                if (!doContinue)
                {
                    Console.WriteLine(msg);
                    break;
                }

                if (!string.IsNullOrEmpty(msg))
                {
                    Console.WriteLine("\t" + msg);
                }
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine(@"Hello. My name is Cozmo. I am a table robot.\nYou can move me on the tablespace 5x5 units.");
            Console.WriteLine(@"I understand the following Commands (CAPITAL LETTERS ONLY, please :-)):");
        }
    }
}
