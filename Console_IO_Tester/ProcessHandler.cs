using System.Diagnostics;
using System.Text;

namespace Console_IO_Tester
{
    internal class ProcessHandler
    {
        public Process Process;
        public StringBuilder Stderrx;
        public StringBuilder Stdoutx;

        public ProcessHandler()
        {
            this.Process = new Process();
            this.Stderrx = new StringBuilder();
            this.Stdoutx = new StringBuilder();
        }

    }
}
