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
        /// <summary>
        /// Toggle default value for using the dotnet CLI for testing in the <see cref="IO_Exception_Check"/> class..
        /// </summary>
        public static bool UseDotnetCLI = true;

        /// <summary>
        /// Toggle using the dotnet CLI in this single instance of <see cref="IO_Exception_Check"/> class.
        /// </summary>
        public bool LocalUseDotnetCLI = UseDotnetCLI;

        /// <summary>
        /// If true only returns test results when a exception has been produced.
        /// </summary>
        public bool ReturnOnlyExceptions = true;

        public string appPath;
        public string arguments;
        public string testInputPath;
        
        public int timeout = 30000;

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
        /// Initializes a new instance of the <see cref="IO_Exception_Check"/> class.
        /// </summary>
        /// <param name="appPath">Path to target application.</param>
        /// <param name="testInputPath">Path to file containing test input strings.</param>
        /// <param name="timeout">Timeout in millisecond till the target application is killed.</param>
        public IO_Exception_Check(string appPath, string testInputPath, int timeout)
        {
            this.appPath = appPath;
            this.testInputPath = testInputPath;
            this.timeout = timeout;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IO_Exception_Check"/> class.
        /// </summary>
        /// <param name="appPath">Path to target application.</param>
        /// <param name="testInputPath">Path to file containing test input strings.</param>
        /// <param name="timeout">Timeout in millisecond till the target application is killed.</param>
        /// <param name="LocalUseDotnetCLI">Toggle using the dotnet CLI in this single instance of <see cref="IO_Exception_Check"/> class.</param>
        public IO_Exception_Check(string appPath, string testInputPath, int timeout, bool LocalUseDotnetCLI)
        {
            this.appPath = appPath;
            this.testInputPath = testInputPath;
            this.timeout = timeout;
            this.LocalUseDotnetCLI = LocalUseDotnetCLI;
        }

        /// <summary>
        /// Inputing all strings from testInputPath into standard input of target application.
        /// </summary>
        /// <returns>true if there are no exception. On exception, current testInput, exception and standard output are returned.</returns>
        public List<IO_Exception_Check_Result> RunCheck()
        {
            List<string> array = JsonConvert.DeserializeObject<List<string>>(System.IO.File.ReadAllText(testInputPath));

            List<IO_Exception_Check_Result> results = new List<IO_Exception_Check_Result>();

            StringBuilder stdoutxTemp = new StringBuilder();
            StringBuilder stderrxTemp = new StringBuilder();

            LauchProcessHandler(ref stdoutxTemp, ref stderrxTemp);

            if (LocalUseDotnetCLI)
            {
                arguments += "--no-build";
            }

            Parallel.ForEach(array, item =>
            {
                StringBuilder stdoutx = new StringBuilder();
                StringBuilder stderrx = new StringBuilder();

                Process process = LauchProcessHandler(ref stdoutx,ref stderrx);
               

                for (int i = 0; i < 20; i++)
                {
                    process.StandardInput.WriteLine(item);
                }

                try
                {
                    if (!process.WaitForExit(timeout))
                    {
                        process.Kill();
                    }
                }
                catch
                {}

                if (!string.IsNullOrEmpty(stderrx.ToString()))
                {
                    results.Add(new IO_Exception_Check_Result(item, stdoutx.ToString(), stderrx.ToString()));
                }
                else
                {
                    if (!ReturnOnlyExceptions)
                    {
                        results.Add(new IO_Exception_Check_Result(item, stdoutx.ToString()));
                    }
                }
            });

            return results;
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

        private Process LauchProcessHandler(ref StringBuilder stdoutxParam, ref StringBuilder stderrxParam)
        {
            StringBuilder stdoutx = new StringBuilder();
            StringBuilder stderrx = new StringBuilder();

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                RedirectStandardError = true,
            };

            if (LocalUseDotnetCLI)
            {
                startInfo.FileName = "dotnet";
                startInfo.Arguments = $"run --project {appPath} {arguments}";
                //startInfo.WorkingDirectory = startInfo.Arguments;
            }
            else
            {
                startInfo.FileName = appPath;
                startInfo.Arguments = arguments;
            }

            Process process = new Process();
            process.StartInfo = startInfo;

            process.Start();

            process.OutputDataReceived += (object sender, System.Diagnostics.DataReceivedEventArgs e) => stdoutx.Append(e.Data);
            process.ErrorDataReceived += (object sender, System.Diagnostics.DataReceivedEventArgs e) => stderrx.Append(e.Data);

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            stdoutxParam = stdoutx;
            stderrxParam = stderrx;

            return process;

        }
    }
}
