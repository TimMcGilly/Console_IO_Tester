using System;

namespace Console_IO_Tester_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console_IO_Tester.IO_Exception_Check Console_Exception_Check = new Console_IO_Tester.IO_Exception_Check(@"..\..\..\..\Null_Reference_Exception_Every_Run", @"..\..\..\10_inputs.json");
            var results = Console_Exception_Check.RunCheck();
            foreach (var result in results)
            {
                Console.WriteLine("Test value: " + result.testInput);
                Console.WriteLine("Output: " + result.output);
                if (result.exception != null)
                {
                    Console.WriteLine("Exception: " + result.exception);
                }
            }
            Console.WriteLine("Finished");
            Console.ReadLine();
        }
    }
}
