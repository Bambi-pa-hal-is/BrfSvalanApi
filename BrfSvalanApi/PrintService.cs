using BrfSvalanApi.Display;
using BrfSvalanApi.Input;
using BrfSvalanApi.Print;
using BrfSvalanApi.Scan;
using Iot.Device.CharacterLcd;
using System.Device.Gpio;
using System.Device.I2c;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BrfSvalanApi
{
    public class PrintService : BackgroundService
    {
        private readonly IDisplay _display;
        private readonly IInputReader _inputReader;

        public PrintService()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _display = new WindowsDisplay();
                _inputReader = new ConsoleKeyReader();
            }
            else
            {
                _display = new LcdDisplay();
                _inputReader = new RotaryEncoder(27, 17, 22);
            }
            Console.WriteLine("print service constructed!");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Executing printservice!");

            var inputManager = new InputManager(_display);
            inputManager.UpdateLastActionTime();
            var mainMenu = new MainMenu(new List<IPipeline>()
            {
                new PrintPipeline(),
                new ScanPipeline()
            });
            _display.SetDefaultDisplay(mainMenu);
            _display.SetDisplay(mainMenu);
            _display.SetBacklight(true);
            _inputReader.RotatedClockwise += (sender, args) => inputManager.HandleRotation(true);
            _inputReader.RotatedCounterClockwise += (sender, args) => inputManager.HandleRotation(false);
            _inputReader.ButtonReleased += (sender, args) => inputManager.HandleSelection();
            _inputReader.ResetEvent += (sender, args) => inputManager.Reset();
            while (!stoppingToken.IsCancellationRequested)
            {
                await _inputReader.Listen(stoppingToken);

                if (DateTime.Now - inputManager.GetLastActionTime() > TimeSpan.FromMinutes(5))
                {
                    // If running on Linux (Raspberry Pi typically runs a version of Linux), shut down
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        _display.SetBacklight(false);
                        _display.ClearDisplay();
                        Process.Start("sudo", "shutdown -h now"); // shutdown the Raspberry Pi
                    }
                }
            }

            // Cleanup resources when the service stops
            DriveManager.Unmount();
            _display.ClearDisplay();
            _display.Dispose();
        }
    }
}
