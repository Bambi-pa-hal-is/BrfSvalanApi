using System.Device.Gpio;

namespace BrfSvalanApi
{
    public class RotaryEncoder
    {
        private readonly GpioController _controller;
        private readonly int _dtPin;
        private readonly int _clkPin;
        private readonly int _swPin;
        private PinValue _lastDtState;

        public event EventHandler RotatedClockwise;
        public event EventHandler RotatedCounterClockwise;
        public event EventHandler ButtonPressed;

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

            _controller.RegisterCallbackForPinValueChangedEvent(_dtPin, PinEventTypes.Falling, RotaryTurned);
            _controller.RegisterCallbackForPinValueChangedEvent(_swPin, PinEventTypes.Falling, ButtonPushed);
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
    }
}
