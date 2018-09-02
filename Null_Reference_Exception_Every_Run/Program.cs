using System;
using System.IO;

namespace Null_Reference_Exception_Every_Run
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();
            Console.WriteLine("Null Reference Exeption test, test output");
            throw new NullReferenceException(); 
        }
    }
}
