using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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

        /// <summary>
        /// Path to target application.
        /// </summary>
        public string appPath;

        /// <summary>
        /// Arugments passed to target application on launch.
        /// </summary>
        public string arguments;

        /// <summary>
        /// Path to file containing test input strings.
        /// </summary>
        public string testInputPath;

        /// <summary>
        /// Timeout in millisecond till the target application is killed.
        /// </summary>
        public int timeout = 60000;

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
        public ConcurrentBag<IO_Exception_Check_Result> Start()
        {
            return Start(Array.Empty<string>());
        }

        /// <summary>
        /// Inputing all strings from testInputPath into standard input of target application.
        /// </summary>
        /// <param name="startingInputs">Input to be sent to the target application before test input are entered.</param>
        /// <returns>true if there are no exception. On exception, current testInput, exception and standard output are returned.</returns>
        public ConcurrentBag<IO_Exception_Check_Result> Start(string[] startingInputs)
        {
            List<string> testInputArray = JsonConvert.DeserializeObject<List<string>>(System.IO.File.ReadAllText(testInputPath));

            int testInputArrayLength = testInputArray.Count;

            var results = new ConcurrentBag<IO_Exception_Check_Result>();

            //Process[] processes = new Process[testInputArray.Count];

            var processes = new ProcessHandler[testInputArrayLength];

            if (LocalUseDotnetCLI)
            {
                //Build the .net application once and then sets the --no-build so it isn't rebuilt
                Dotnet_CLI_build();
                arguments += "--no-build";
            }

            //Launches process for each item in the testInputArray
            for (int i = 0; i < testInputArrayLength; i++)
            {
                //Catches exceptions when launching proccess and sending them input
                
                    processes[i] = LauchProcessHandler();

                    foreach (var input in startingInputs)
                    {
                        processes[i].process.StandardInput.WriteLine(input);
                    }

                    for (int x = 0; x < 20; x++)
                    {
                        processes[i].process.StandardInput.WriteLine(testInputArray[i]);
                    }
            }
            Console.WriteLine("launched");


            //Cleans up processes and adds any exceptions to results
            for (int i = 0; i < processes.Length; i++)
            {
                try
                {
                    if (!processes[i].process.WaitForExit(timeout))
                    {
                        processes[i].process.Kill();
                    }
                }
                catch (Exception e)
                {
                    //TODO: Add proper logging functionatly. 
                    Debug.WriteLine(e);
                }

                if (!string.IsNullOrEmpty(processes[i].stderrx.ToString()))
                {
                    results.Add(new IO_Exception_Check_Result(testInputArray[i], processes[i].stdoutx.ToString(), processes[i].stderrx.ToString()));
                }
                else
                {
                    if (!ReturnOnlyExceptions)
                    {
                        results.Add(new IO_Exception_Check_Result(testInputArray[i], processes[i].stdoutx.ToString()));
                    }
                }
            }

            return results;
        }

        private ProcessHandler LauchProcessHandler()
        {
            var processHandler = new ProcessHandler();

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
            }
            else
            {
                startInfo.FileName = appPath;
                startInfo.Arguments = arguments;
            }


            processHandler.process.StartInfo = startInfo;

            processHandler.process.OutputDataReceived += (object sender, System.Diagnostics.DataReceivedEventArgs e) => processHandler.stdoutx.Append(e.Data);
            processHandler.process.ErrorDataReceived += (object sender, System.Diagnostics.DataReceivedEventArgs e) => processHandler.stderrx.Append(e.Data);



            processHandler.process.Start();

            processHandler.process.BeginOutputReadLine();
            processHandler.process.BeginErrorReadLine();

            return processHandler;
        }

        private void Dotnet_CLI_build()
        {
            var startInfo = new ProcessStartInfo { UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardInput = true, FileName = "dotnet", Arguments = $"build {appPath}" };
            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();

                process.WaitForExit();
            }

        }
    }
}
