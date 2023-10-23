using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BrfSvalanApi
{
    public static class DriveManager
    {
        private static readonly string[] TargetDevices = { "/dev/sda1", "/dev/sdb1", "/dev/sdc1" };

        public static bool IsUsbMounted = false;

        public static string GetMountPoint()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return @"C:\temp";
            }
            else
            {
                return "/media/printdrive";
            }
        }

        public static bool Mount()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return true;
            }

            EnsureMountPointExists();
            if (IsDriveMounted())
            {
                Unmount();
            }


            string deviceToMount = GetDeviceToMount();
            if (!string.IsNullOrEmpty(deviceToMount))
            {
                RunCommand($"sudo mount -o uid=1000,gid=1000 {deviceToMount} {GetMountPoint()}");
                Console.WriteLine($"Mounted drive {deviceToMount}!!");
                IsUsbMounted = true;
                return true;
            }
            else
            {
                Console.WriteLine("Failed to mount drives");
                IsUsbMounted = false;
                return false;
            }
        }

        private static void EnsureMountPointExists()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return;
            }
            if (!Directory.Exists(GetMountPoint()))
            {
                RunCommand($"sudo mkdir -p {GetMountPoint()}");
            }
        }

        public static bool IsDriveMounted()
        {
            string output = RunCommand($"mount | grep {GetMountPoint()}");
            return !string.IsNullOrEmpty(output);
        }

        public static void Unmount()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return;
            }
            EnsureMountPointExists();
            if (IsDriveMounted())
            {
                Console.WriteLine("Unmounting now!");
                RunCommand($"sudo umount {GetMountPoint()}");
                Console.WriteLine("Unmounting complete!");
            }
            else
            {
                Console.WriteLine("Drive is not mounted!");
            }
            IsUsbMounted = false;
        }

        private static string GetDeviceToMount()
        {
            string output = RunCommand("lsblk");

            foreach (var device in TargetDevices)
            {
                var deviceWithoutDev = device.Replace("/dev/", "");
                if (output.Contains(deviceWithoutDev))
                {
                    return device;
                }
            }

            return string.Empty;  // Return an empty string if no device is found
        }

        private static string RunCommand(string command)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{command}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }
    }
}
