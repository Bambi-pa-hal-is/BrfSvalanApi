using System.Diagnostics;

namespace BrfSvalanApi
{
    public class DriveManager
    {
        private readonly string[] targetDevices = { "/dev/sda1", "/dev/sdb1", "/dev/sdc1" };
        public static string MountPoint = "/media/printdrive";

        public bool Mount()
        {
            EnsureMountPointExists();
            string deviceToMount = GetDeviceToMount();

            if (!string.IsNullOrEmpty(deviceToMount))
            {
                RunCommand($"sudo mount -o uid=1000,gid=1000 {deviceToMount} {MountPoint}");
                return true;
            }
            else
            {
                return false;
            }
        }

        private void EnsureMountPointExists()
        {
            if (!Directory.Exists(MountPoint))
            {
                RunCommand($"sudo mkdir -p {MountPoint}");
            }
        }

        public void Unmount()
        {
            RunCommand($"sudo umount {MountPoint}");
        }

        private string GetDeviceToMount()
        {
            string output = RunCommand("lsblk");

            foreach (var device in targetDevices)
            {
                if (output.Contains(device))
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
