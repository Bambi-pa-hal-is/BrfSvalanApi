using System.Device.I2c;

namespace BrfSvalanApi
{
    public class I2cDisplay
    {
        private const int I2CAddress = 0x27;
        private readonly I2cConnectionSettings _settings;
        private readonly I2cDevice _device;

        public I2cDisplay()
        {
            _settings = new I2cConnectionSettings(1, I2CAddress); // 1 represents the I2C Bus ID. It's 1 for Raspberry Pi 3/4.
            _device = I2cDevice.Create(_settings);
        }

        public void InitializeLCD()
        {
            System.Threading.Thread.Sleep(50); // Wait for more than 40ms after power is applied
            Send(0x30, false); // Function set command
            System.Threading.Thread.Sleep(5); // Wait for more than 4.1ms
            Send(0x30, false); // Function set command
            System.Threading.Thread.Sleep(1); // Wait for more than 100μs
            Send(0x30, false); // Function set command
            System.Threading.Thread.Sleep(1);

            // The LCD is now in 8-bit mode
            Send(0x20, false); // We only want 4-bit mode
            Send(0x20 | (1 << 2), false); // 4-bit mode, 2-line display, 5x8 font
            Send(0x08, false); // Display off
            Send(0x01, false); // Clear display
            System.Threading.Thread.Sleep(2); // Clear display is slow
            Send(0x06, false); // Entry mode: Left-to-right, no shift
            Send(0x0C, false); // Display on, no cursor, no blink
        }

        private void PulseEnable(byte data)
        {
            _device.WriteByte(data);
            System.Threading.Thread.Sleep(1);
            _device.WriteByte((byte)(data | (1 << 5))); // Enable E with P5=1
            System.Threading.Thread.Sleep(1);
            _device.WriteByte(data);
        }

        private void Send(byte data, bool isData)
        {
            byte highNibble = (byte)(data & 0xF0);
            byte lowNibble = (byte)((data << 4) & 0xF0);

            if (isData)
                highNibble |= (1 << 7); // RS = 1 for data
            if (isData)
                lowNibble |= (1 << 7); // RS = 1 for data

            PulseEnable(highNibble);
            PulseEnable(lowNibble);
        }

        public void SetBacklight(bool backlight)
        {
            
        }

        public void SetCursor()
        {

        }

        public void Write(string message)
        {
            foreach (char c in message)
            {
                Send((byte)c, true);
            }
        }
    }
}
