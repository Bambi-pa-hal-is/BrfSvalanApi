using System.Device.Gpio;

namespace BrfSvalanApi.Input
{
    public class RotaryEncoder : IInputReader
    {
        private readonly GpioController _controller;
        private readonly int _dtPin;
        private readonly int _clkPin;
        private readonly int _swPin;
        private PinValue _lastDtState;

        public event EventHandler RotatedClockwise;
        public event EventHandler RotatedCounterClockwise;
        public event EventHandler ButtonPressed;
        public event EventHandler ButtonReleased;

        public RotaryEncoder(int dtPin, int clkPin, int swPin)
        {
            _dtPin = dtPin;
            _clkPin = clkPin;
            _swPin = swPin;

            _controller = new GpioController();

            _controller.OpenPin(_dtPin, PinMode.InputPullUp);
            _controller.OpenPin(_clkPin, PinMode.InputPullUp);
            _controller.OpenPin(_swPin, PinMode.InputPullUp);

            _lastDtState = _controller.Read(_dtPin);

            _controller.RegisterCallbackForPinValueChangedEvent(_dtPin, PinEventTypes.Falling | PinEventTypes.Rising, RotaryTurned);
            _controller.RegisterCallbackForPinValueChangedEvent(_swPin, PinEventTypes.Falling, ButtonPushed);
            _controller.RegisterCallbackForPinValueChangedEvent(_swPin, PinEventTypes.Rising, ButtonReleasedCallback);
        }

        private void RotaryTurned(object sender, PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            var currentDtState = _controller.Read(_dtPin);
            if (_lastDtState != currentDtState)
            {
                if (_controller.Read(_clkPin) != currentDtState)
                {
                    RotatedClockwise?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    RotatedCounterClockwise?.Invoke(this, EventArgs.Empty);
                }
            }

            _lastDtState = currentDtState;
        }

        private void ButtonPushed(object sender, PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            ButtonPressed?.Invoke(this, EventArgs.Empty);
        }

        private void ButtonReleasedCallback(object sender, PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            ButtonReleased?.Invoke(this, EventArgs.Empty);
        }

        public async Task StartListening(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1, stoppingToken);  // Poll every 100ms, adjust as needed
            }
        }
    }
}
