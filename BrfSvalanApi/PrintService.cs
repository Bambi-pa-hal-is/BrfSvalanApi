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
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var currentValue = 0;
            var prevValue = 0;
            Console.WriteLine("Executing printservice!");
            _rotaryEncoder.RotatedClockwise += (sender, x) =>
            {
                currentValue++;
            };
            _rotaryEncoder.RotatedCounterClockwise += (sender, x) =>
            {
                currentValue--;
            };
            _rotaryEncoder.ButtonPressed += (sender, x) =>
            {
                currentValue += 100;
            };
            while (!stoppingToken.IsCancellationRequested)
            {
                if (currentValue != prevValue)
                {
                    _lcd.ClearDisplay();
                    _lcd.Write($"Position: {currentValue}");
                    prevValue = currentValue;
                }
                await Task.Delay(10, stoppingToken);  // Poll every 100ms, adjust as needed
            }

            // Cleanup resources when the service stops
            _lcd.ClearDisplay();
            _lcd.Dispose();
        }
    }
}
