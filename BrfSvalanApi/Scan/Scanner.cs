using static BrfSvalanApi.Scan.ScanPipeline;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BrfSvalanApi.Scan
{
    public static class Scanner
    {
        public static void Scan(ScanProperties properties)
        {
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            // Generate a unique filename based on the current date and time
            string filename = $"{DateTime.Now:yyyyMMdd_HHmmss}.{properties.FileFormat}";
            string outputPath = Path.Combine(properties.Location, filename);

            // Construct the command
            string cmd = $"scanimage --resolution {properties.Resolution} --format {properties.FileFormat} > {outputPath}";
            Console.WriteLine(cmd);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return;
            }
            // Set up and start the process
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{cmd}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                process.Start();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new Exception($"scanimage command failed with exit code {process.ExitCode}");
                }
            }
        }
    }
}
