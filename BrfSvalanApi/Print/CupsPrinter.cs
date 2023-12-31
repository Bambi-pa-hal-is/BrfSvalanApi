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
            //Check if docx or any other word format here
            bool hasConvertedToPdf = false;
            if (Path.GetExtension(properties.File).ToLower() == ".docx")
            {
                if (!ConvertToPdf(properties))
                {
                    Console.WriteLine("Failed to convert to PDF.");
                    return false;
                }
                Console.WriteLine("Converted to pdf!");
                hasConvertedToPdf = true;
            }

            var filePath = properties.File.Replace(" ", "\\ ");
            var command = $"lp -d {PrinterName} -n {properties.Copies} {filePath}";
            Console.WriteLine(command);

            CancelAllJobs();
            ExecuteShellCommand(command);
            if(hasConvertedToPdf)
            {
                try
                {
                    Thread.Sleep(5000);
                    File.Delete(properties.File);
                    Console.WriteLine($"Deleted PDF file: {properties.File}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting PDF file: {ex.Message}");
                    // You might want to decide how to handle this. 
                    // For example, you could return false if file deletion is critical.
                }
            }
            return true;
        }

        public static bool ConvertToPdf(PrintProperties properties)
        {
            var inputPath = properties.File;
            var outputPath = Path.ChangeExtension(inputPath, ".pdf");
            var workingDir = Path.GetDirectoryName(inputPath);

            var command = $"libreoffice --headless --convert-to pdf '{Path.GetFileName(inputPath)}' --outdir \"{workingDir}\"";
            Console.WriteLine(command);

            var processInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{command}\"",
                WorkingDirectory = workingDir,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            try
            {
                using (var process = Process.Start(processInfo))
                {
                    var output = process.StandardOutput.ReadToEnd();
                    var error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    Console.WriteLine("Output: " + output);
                    Console.WriteLine("Error: " + error);

                    if (process.ExitCode == 0 && File.Exists(outputPath))
                    {
                        properties.File = outputPath; // Update the file path to the new PDF
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during PDF conversion: " + ex.Message);
            }

            return false;
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
