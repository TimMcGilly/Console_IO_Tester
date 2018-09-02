using System;
using Xunit;

namespace Console_IO_Tester.Test
{
    public class Console_IO_Tester_Tests
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(2, 1 + 1);
        }

        [Fact]
        public void Simple_10_input_validation()
        {
            Console_IO_Tester.IO_Exception_Check Console_Exception_Check = new Console_IO_Tester.IO_Exception_Check(@"..\..\..\..\Null_Reference_Exception_Every_Run", @"..\..\..\10_inputs.json");
            var results = Console_Exception_Check.RunCheck();
            Assert.True(results.Count > 0);
        }
    }
}
