using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bee_backend
{
    internal class Program
    {
        const uint CP_UTF8 = 65001;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleOutputCP(uint wCodePageID);
        static int Main(string[] args)
        {
            SetConsoleOutputCP(CP_UTF8);

            //string curWorkingDir = Process.GetCurrentProcess().StartInfo.WorkingDirectory;
            //Console.WriteLine($"Current working directory: {curWorkingDir}");

            // 获取当前exe的完整路径
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            // 获取当前exe所在目录
            string exeDirectory = System.IO.Path.GetDirectoryName(exePath);

            StringBuilder exeSb = new StringBuilder();
            exeSb.Append("\"");
            exeSb.Append(exeDirectory);
            exeSb.Append("\\");
            exeSb.Append("bee_backend_real.exe");
            exeSb.Append("\"");

            StringBuilder sb = new StringBuilder();
            for (int index = 0; index < args.Length; index++)
            {
                if (args[index] != "--stdin-canary")
                {
                    sb.Append(" ");
                    sb.Append(args[index]);
                }
            }
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                UseShellExecute = false,
                FileName = exeSb.ToString(),
                Arguments = sb.ToString(),
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                ErrorDialog = false,
                WorkingDirectory = string.Empty,
            };
            Console.WriteLine($"Starting process: {psi.FileName} {psi.Arguments}");
            using( var process = Process.Start( psi ) ) 
            using( ManualResetEvent mreOut = new ManualResetEvent(false),
                mreErr = new ManualResetEvent(false))
            {
                //process.OutputDataReceived += (sender, e) =>
                //{
                //    if (e.Data != null)
                //    {
                //        Console.Error.WriteLine(e.Data);
                //    }
                //    else
                //    {
                //        mreErr.Set();
                //    }
                //};
                //process.BeginOutputReadLine();
                //process.ErrorDataReceived += (sender, e) =>
                //{
                //    if (e.Data != null)
                //    {
                //        Console.Error.WriteLine(e.Data);
                //    }
                //    else
                //    {
                //        mreErr.Set();
                //    }
                //};
                //process.BeginErrorReadLine();

                StreamReader reader = process.StandardOutput;
                string output = reader.ReadToEnd();
                Console.WriteLine(output);

                process.WaitForExit();

                //mreErr.WaitOne();
                //mreOut.WaitOne();

                return process.ExitCode;
            }
        }
    }
}
