using System;

namespace StartingArgumentsException
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine();
            if (input == "a")
            {
                input = Console.ReadLine();
                if (input == "b")
                {
                    Console.WriteLine("Throwing InvalidOperationException");
                    throw new InvalidOperationException();
                }
                else
                {
                    Console.WriteLine("Wrong starting inputs for exception.");
                }
            }
            else
            {
                Console.WriteLine("Wrong starting inputs for exception.");
            }
        }
    }
}
