using System;
using System.Collections.Generic;
using System.Text;

namespace Console_IO_Tester
{
    public class IO_Exception_Check_Result
    {
        public readonly string testInput;
        public readonly string output;
        public readonly string exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="IO_Exception_Check_Result"/> class.
        /// </summary>
        /// <param name="testInput">Test value inputed into targeted application</param>
        /// <param name="output">Output from target application</param>
        public IO_Exception_Check_Result(string testInput, string output)
        {
            this.testInput = testInput;
            this.output = output;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IO_Exception_Check_Result"/> class.
        /// </summary>
        /// <param name="testInput">Test value inputed into targeted application</param>
        /// <param name="output">Output from target application</param>
        /// <param name="exception">Exception returned from target application</param>
        public IO_Exception_Check_Result(string testInput, string output, string exception)
        {
            this.testInput = testInput;
            this.output = output;
            this.exception = exception;
        }
    }
}
