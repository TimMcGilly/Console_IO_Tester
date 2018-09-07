using System;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace Console_IO_Tester.Test
{
    public class Console_IO_Tester_Tests
    {

        private readonly ITestOutputHelper output;

        public Console_IO_Tester_Tests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Test1()
        {
            Assert.Equal(2, 1 + 1);
        }

        [Fact]
        public void Ten_inputs_all__null_reference_exceptions()
        {
            IO_Exception_Check Console_Exception_Check = new IO_Exception_Check("../../../../Null_Reference_Exception_Every_Run", "../../../10_inputs.json");

            var results = Console_Exception_Check.Start();
            Assert.Equal(10, results.Count);
        }

        [Fact]
        public void BLNS_all_exceptions()
        {
            IO_Exception_Check Console_Exception_Check = new IO_Exception_Check("../../../../Null_Reference_Exception_Every_Run", "../../../blns.json");

            var results = Console_Exception_Check.Start();
            Assert.Equal(503, results.Count);
        }

        [Fact]
        public void Ten_inputs_no_exceptions()
        {
            IO_Exception_Check Console_Exception_Check = new IO_Exception_Check("../../../../No_Exceptions", "../../../10_inputs.json");

            var results = Console_Exception_Check.Start();
            Assert.Empty(results);
        }

        [Fact]
        public void BLNS_no_exceptions()
        {
            IO_Exception_Check Console_Exception_Check = new IO_Exception_Check("../../../../No_Exceptions", "../../../blns.json");

            var results = Console_Exception_Check.Start();
            Assert.Empty(results);
        }

        [Fact]
        public void Starting_Inputs_Exception_Test()
        {
            IO_Exception_Check Console_Exception_Check = new IO_Exception_Check("../../../../StartingArgumentsException", "../../../10_inputs.json");

            string[] inputs = { "a", "b" };
            var results = Console_Exception_Check.Start(inputs);
            
            Assert.Equal(10, results.Count);
        }

        [Fact]
        public void Starting_Inputs_No_Exception_Test()
        {
            IO_Exception_Check Console_Exception_Check = new IO_Exception_Check("../../../../StartingArgumentsException", "../../../10_inputs.json");

            string[] inputs = { "b", "c" };
            var results = Console_Exception_Check.Start(inputs);
            foreach (var result in results)
            {
                output.WriteLine("Test value: " + result.testInput);
                output.WriteLine("Output: " + result.output);
                if (result.exception != null)
                {
                    output.WriteLine("Exception: " + result.exception);
                }
            }
            Assert.Empty(results);
        }

        [Fact]
        public void Start_Running_Event_Test()
        {
            var console_Exception_Check = new IO_Exception_Check("../../../../No_Exceptions", "../../../10_inputs.json");
             console_Exception_Check.Start();

            console_Exception_Check.StartExited += (object sender, EventArgs e) =>
            {
                Assert.Empty(console_Exception_Check.Start()); 
            };
        }

        [Fact]
        public void Start_Running_Exception_Test()
        {
            var console_Exception_Check = new IO_Exception_Check("../../../../No_Exceptions", "../../../10_inputs.json");

            ThreadStart threadStart = new ThreadStart(() => console_Exception_Check.Start());
            Thread thread = new Thread(threadStart);
            thread.Start();
            Exception ex = Assert.Throws<IO_Exception_Check.StartAlreadyRunning>(() => console_Exception_Check.Start());

            Assert.Equal("Start() is already running. Please use the StartExited event.", ex.Message);

            
        }
    }
}
