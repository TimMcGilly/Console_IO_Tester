
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_IO_Tester_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console_IO_Tester.IO_Exception_Check io_Exception_Check = new Console_IO_Tester.IO_Exception_Check(@"C:\Users\timmc_000\Source\Repos\Challenges_C_Sharp\Challenges_C_Sharp\bin\Debug\Challenges_C_Sharp.exe");
            var result = io_Exception_Check.RunCheck();
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
