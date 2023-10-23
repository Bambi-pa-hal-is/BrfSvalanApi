using BrfSvalanApi.Display;
using BrfSvalanApi.Input;
using BrfSvalanApi.Print;
using Iot.Device.CharacterLcd;
using System.Device.Gpio;
using System.Device.I2c;
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

            var applicationDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //var printSubMenu = new FileMenu(applicationDirectory);

            //var mainMenu = new Menu("Vad vill du göra?");
            //mainMenu.Items.Add(new MenuItem("Print", () => { }, printSubMenu));
            //mainMenu.Items.Add(new MenuItem("Scan", () => { }));

            // Add USB files here. Each MenuItem could be like new MenuItem("File1.txt", SelectFileAction);

            var inputManager = new InputManager(_display);
            var mainMenu = new MainMenu(new List<IPipeline>()
            {
                new PrintPipeline()
            });
            _display.SetDefaultDisplay(mainMenu);
            _display.SetDisplay(mainMenu);
            _inputReader.RotatedClockwise += (sender, args) => inputManager.HandleRotation(true);
            _inputReader.RotatedCounterClockwise += (sender, args) => inputManager.HandleRotation(false);
            _inputReader.ButtonReleased += (sender, args) => inputManager.HandleSelection();
            await _inputReader.StartListening(stoppingToken);

            // Cleanup resources when the service stops
            _display.ClearDisplay();
            _display.Dispose();
        }
    }
}
