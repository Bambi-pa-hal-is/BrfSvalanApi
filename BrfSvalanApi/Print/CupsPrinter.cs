﻿using static BrfSvalanApi.Print.PrintPipeline;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BrfSvalanApi.Print
{
    public class CupsPrinter
    {
        private const string PrinterName = "epson";

        public static bool Print(PrintProperties properties)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return true;
            }
            if (string.IsNullOrEmpty(properties.File))
            {
                Console.WriteLine(properties.File + " Is this really null?");
                throw new ArgumentException("File path cannot be null or empty.");
            }

            var command = $"lp -d {PrinterName} -n {properties.Copies} {properties.File}";
            Console.WriteLine(command);

            CancelAllJobs();
            ExecuteShellCommand(command);
            return true;
        }

        public static void CancelAllJobs()
        {
            var command = $"cancel -a {PrinterName}";
            ExecuteShellCommand(command);
        }

        private static bool IsPrinterIdle()
        {
            var statusCommand = $"lpstat -p {PrinterName}";
            
            var statusOutput = ExecuteShellCommand(statusCommand);
            Console.WriteLine(statusOutput);
            if (statusOutput.Contains("idle", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            return false;
        }

        private static string ExecuteShellCommand(string command)
        {
            var processInfo = new ProcessStartInfo("bash", $"-c \"{command}\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(processInfo);
            string output = "";

            if (process != null)
            {
                output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new InvalidOperationException($"Command `{command}` exited with code {process.ExitCode}");
                }
            }
            return output.Trim();
        }
    }
}
