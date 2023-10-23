using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BrfSvalanApi
{
    public class DriveManager
    {
        private readonly string[] targetDevices = { "/dev/sda1", "/dev/sdb1", "/dev/sdc1" };
        //public static string MountPoint = "/media/printdrive";
        //public static string MountPoint2 = "/media/printdrive";

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

        public bool Mount()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return true;
            }
            EnsureMountPointExists();
            string deviceToMount = GetDeviceToMount();
            Unmount();
            if (!string.IsNullOrEmpty(deviceToMount))
            {
                RunCommand($"sudo mount -o uid=1000,gid=1000 {deviceToMount} {GetMountPoint()}");
                Console.WriteLine($"Mounted drive {deviceToMount}");
                return true;
            }
            else
            {
                Console.WriteLine("Failed to mount drives");
                return false;
            }
        }

        private void EnsureMountPointExists()
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

        public void Unmount()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return;
            }
            Console.WriteLine("Unmounting now!");
            RunCommand($"sudo umount {GetMountPoint()}");
            Console.WriteLine("Unmounting complete!");
        }

        private string GetDeviceToMount()
        {
            string output = RunCommand("lsblk");

            foreach (var device in targetDevices)
            {
                var deviceWithoutDev = device.Replace("/dev/", "");
                if (output.Contains(deviceWithoutDev))
                {
                    return device;
                }
            }

            return string.Empty;  // Return an empty string if no device is found
        }

        private string RunCommand(string command)
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
