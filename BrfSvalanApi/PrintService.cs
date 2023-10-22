using BrfSvalanApi.Display;
using BrfSvalanApi.Print;
using Iot.Device.CharacterLcd;
using System.Device.Gpio;
using System.Device.I2c;

namespace BrfSvalanApi
{
    public class PrintService : BackgroundService
    {
        private readonly LcdDisplay _lcd;
        private readonly RotaryEncoder _rotaryEncoder;

        public PrintService()
        {
            Console.WriteLine("print service constructed!");
            _lcd = new LcdDisplay();
            _rotaryEncoder = new RotaryEncoder(17, 27, 22);
            _rotaryEncoder = new RotaryEncoder(27, 17, 22);
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

            var inputManager = new InputManager(_lcd);
            var mainMenu = new MainMenu(new List<IPipeline>()
            {
                new PrintPipeline()
            });
            _lcd.SetDefaultDisplay(mainMenu);
            _lcd.SetDisplay(mainMenu);
            _rotaryEncoder.RotatedClockwise += (sender, args) => inputManager.HandleRotation(true);
            _rotaryEncoder.RotatedCounterClockwise += (sender, args) => inputManager.HandleRotation(false);
            _rotaryEncoder.ButtonReleased += (sender, args) => inputManager.HandleSelection();

            while (!stoppingToken.IsCancellationRequested)
            {
                //if (currentValue != prevValue)
                //{
                //    _lcd.ClearDisplay();
                //    _lcd.Write($"Position: {currentValue}");
                //    prevValue = currentValue;
                //}
                await Task.Delay(1, stoppingToken);  // Poll every 100ms, adjust as needed
            }

            // Cleanup resources when the service stops
            _lcd.ClearDisplay();
            _lcd.Dispose();
        }
    }
}
