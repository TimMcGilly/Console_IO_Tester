using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Console_IO_Tester
{
    class ProcessHandler
    {
        public Process process;
        public StringBuilder stderrx;
        public StringBuilder stdoutx;

        public ProcessHandler()
        {
            this.process = new Process();
            this.stderrx = new StringBuilder();
            this.stdoutx = new StringBuilder();
        }

    }
}
