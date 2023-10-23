using static BrfSvalanApi.Print.PrintPipeline;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BrfSvalanApi.Print
{
    public class CupsPrinter
    {
        private const string PrinterName = "epson";

        public static void Print(PrintProperties properties)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return;
            }
            if (string.IsNullOrEmpty(properties.File))
            {
                throw new ArgumentException("File path cannot be null or empty.");
            }

            var command = $"lp -d {PrinterName} -n {properties.Copies} {properties.File}";
            Console.WriteLine(command);

            ExecuteShellCommand(command);
        }

        private static void ExecuteShellCommand(string command)
        {
            var processInfo = new ProcessStartInfo("bash", $"-c \"{command}\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(processInfo);
            if( process != null )
            {
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new InvalidOperationException($"Command `{command}` exited with code {process.ExitCode}");
                }
            }
        }
    }
}
