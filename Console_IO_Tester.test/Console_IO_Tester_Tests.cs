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
        public void Ten_inputs_all__null_reference_exceptions()
        {
            IO_Exception_Check Console_Exception_Check = new IO_Exception_Check("../../../../Null_Reference_Exception_Every_Run", "../../../10_inputs.json");

            var results = Console_Exception_Check.RunCheck();
            Assert.Equal(10, results.Count);
        }

        [Fact]
        public void BLNS_all_exceptions()
        {
            IO_Exception_Check Console_Exception_Check = new IO_Exception_Check("../../../../Null_Reference_Exception_Every_Run", "../../../blns.json");

            var results = Console_Exception_Check.RunCheck();
            Assert.Equal(503, results.Count);
        }

        [Fact]
        public void Ten_inputs_no_exceptions()
        {
            IO_Exception_Check Console_Exception_Check = new IO_Exception_Check("../../../../No_Exceptions", "../../../10_inputs.json");

            var results = Console_Exception_Check.RunCheck();
            Assert.Empty(results);
        }

        [Fact]
        public void BLNS_no_exceptions()
        {
            IO_Exception_Check Console_Exception_Check = new IO_Exception_Check("../../../../No_Exceptions", "../../../blns.json");

            var results = Console_Exception_Check.RunCheck();
            Assert.Empty(results);
        }
    }
}
