using Console_IO_Tester;
using System;

namespace Console_IO_Tester_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            IO_Exception_Check Console_Exception_Check = new IO_Exception_Check("../../../../StartingArgumentsException", "../../../10_inputs.json");

            

            string[] inputs = { "b", "c" };
            var results = Console_Exception_Check.Start(inputs);
            foreach (var result in results)
            {
                Console.WriteLine("Test value: " + result.testInput);
                Console.WriteLine("Output: " + result.output);
                if (result.exception != null)
                {
                    Console.WriteLine("Exception: " + result.exception);
                }
            }
        }
    }
}
