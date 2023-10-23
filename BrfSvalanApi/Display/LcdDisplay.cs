using Iot.Device.CharacterLcd;
using Iot.Device.Pcx857x;
using System.Device.Gpio;
using System.Device.I2c;

namespace BrfSvalanApi.Display
{
    public class LcdDisplay : IDisposable, IDisplay
    {
        private I2cDevice _i2c;
        private Pcf8574 _driver;
        private Lcd1602 _lcd;
        private IRenderableComponent? _renderableComponent { get; set; }
        private IRenderableComponent? _defaultRenderableComponent { get; set; }

        public LcdDisplay()
        {
            _i2c = I2cDevice.Create(new I2cConnectionSettings(1, 0x27));
            _driver = new Pcf8574(_i2c);
            _lcd = new Lcd1602(
                registerSelectPin: 0,
                enablePin: 2,
                dataPins: new int[] { 4, 5, 6, 7 },
                backlightPin: 3,
                backlightBrightness: 0.1f,
                readWritePin: 1,
                controller: new GpioController(PinNumberingScheme.Logical, _driver)
            );
        }

        public void Write(string value)
        {
            _lcd.SetCursorPosition(0, 0);
            _lcd.Write(value);
        }

        public IRenderableComponent? GetDisplayedComponent()
        {
            return _renderableComponent;
        }

        public void SetDisplay(IRenderableComponent renderableComponent)
        {
            _renderableComponent = renderableComponent;
            Update();
        }

        public void Update()
        {
            if (_renderableComponent != null)
            {
                _renderableComponent.Render(this);
            }
        }

        public void Render(IRenderableComponent? renderableComponent)
        {
            if (renderableComponent != null)
            {
                renderableComponent.Render(this);
            }
        }

        public void SetBacklight(bool state)
        {
            _lcd.BacklightOn = state;
        }

        public void Write(int x, int y, string value)
        {
            _lcd.SetCursorPosition(x, y);
            _lcd.Write(value);
        }

        public void ClearDisplay()
        {
            _lcd.Clear();
        }

        public void Dispose()
        {
            _lcd?.Dispose();
            _driver?.Dispose();
            _i2c?.Dispose();
        }

        public void SetDefaultDisplay(IRenderableComponent renderableComponent)
        {
            _defaultRenderableComponent = renderableComponent;
        }

        public void GoToDefaultDisplay()
        {
            if(_defaultRenderableComponent != null)
            {
                SetDisplay(_defaultRenderableComponent);
                _defaultRenderableComponent.Load();
                Render(_defaultRenderableComponent);
            }
        }
    }
}
