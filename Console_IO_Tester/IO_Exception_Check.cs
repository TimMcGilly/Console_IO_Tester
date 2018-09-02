using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Console_IO_Tester
{
    public class IO_Exception_Check
    {
        private string appPath;
        private string testInputPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="IO_Exception_Check"/> class.
        /// </summary>
        /// <param name="appPath">Path to target application.</param>
        /// <param name="testInputPath">Path to file containing test input strings.</param>
        public IO_Exception_Check(string appPath, string testInputPath)
        {
            this.appPath = appPath;
            this.testInputPath = testInputPath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IO_Exception_Check"/> class. Uses Big List of Naughty Strings are input string for test.
        /// </summary>
        /// <param name="appPath">Path to file containing test input strings.</param>

        public IO_Exception_Check(string appPath)
        {
            this.appPath = appPath;
            this.testInputPath = @"C:\Users\timmc_000\Source\Repos\Challenges_C_Sharp\blns.json";
        }

        /// <summary>
        /// Inputing all strings from testInputPath into standard input of target application.
        /// </summary>
        /// <returns>true if there are no exception. On exception, current testInput, exception and standard output are returned.</returns>
        public object RunCheck()
        {
            List<string> array = JsonConvert.DeserializeObject<List<string>>(System.IO.File.ReadAllText(testInputPath));

            StringBuilder result = new StringBuilder("true");

            Parallel.ForEach(array, item =>
            {
                StringBuilder stdoutx = new StringBuilder();
                StringBuilder stderrx = new StringBuilder();

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    FileName = this.appPath
                };

                Process process = new Process();
                process.StartInfo = startInfo;

                process.Start();

                process.OutputDataReceived += (object sender, System.Diagnostics.DataReceivedEventArgs e) => stdoutx.Append(e.Data);
                process.ErrorDataReceived += (object sender, System.Diagnostics.DataReceivedEventArgs e) => stderrx.Append(e.Data);

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                for (int i = 0; i < 20; i++)
                {
                    process.StandardInput.WriteLine(item);
                }

                if (!process.WaitForExit(1000))
                {
                    process.Kill();
                }

                if (!string.IsNullOrEmpty(stderrx.ToString()))
                {
                    result.Append("Input: ").Append(item).Append(" Error: ").Append(stderrx).Append("\nOutput: \n").Append(stdoutx).AppendLine();
                }
            });

            return result.ToString();
        }

        /// <summary>
        /// Inputing all strings from testInputPath into standard input of target application.
        /// </summary>
        /// <param name="startingInputs">Input to be sent to the target application before test input are entered.</param>
        /// <returns>true if there are no exception. On exception, current testInput, exception and standard output are returned.</returns>
        public object RunCheck(string[] startingInputs)
        {
            List<string> array = JsonConvert.DeserializeObject<List<string>>(System.IO.File.ReadAllText(testInputPath));

            StringBuilder result = new StringBuilder("true");

            Parallel.ForEach(array, item =>
            {
                StringBuilder stdoutx = new StringBuilder();
                StringBuilder stderrx = new StringBuilder();

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    FileName = this.appPath
                };

                Process process = new Process();
                process.StartInfo = startInfo;

                process.Start();

                process.OutputDataReceived += (object sender, System.Diagnostics.DataReceivedEventArgs e) => stdoutx.Append(e.Data);
                process.ErrorDataReceived += (object sender, System.Diagnostics.DataReceivedEventArgs e) => stderrx.Append(e.Data);

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                foreach (var input in startingInputs)
                {
                    process.StandardInput.WriteLine(input);
                }

                for (int i = 0; i < 20; i++)
                {
                    process.StandardInput.WriteLine(item);
                }

                if (!process.WaitForExit(1000))
                {
                    process.Kill();
                }

                if (!string.IsNullOrEmpty(stderrx.ToString()))
                {
                    result.Append("Input: ").Append(item).Append(" Error: ").Append(stderrx).Append("\nOutput: \n").Append(stdoutx).AppendLine();
                }
            });

            return result.ToString();
        }
    }
}
