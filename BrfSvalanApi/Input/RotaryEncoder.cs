using System.Device.Gpio;

namespace BrfSvalanApi.Input
{
    using System;
    using System.Device.Gpio;
    using System.Threading;
    using System.Threading.Tasks;

    public class RotaryEncoder : IInputReader
    {
        private readonly GpioController _controller;
        private readonly int _dtPin;
        private readonly int _clkPin;
        private readonly int _swPin;
        private PinValue _lastDtState;
        private DateTime? _buttonPressTimestamp;
        private bool _buttonHeldDown;
        private bool _buttonWasQuicklyReleased = false;
        private CancellationTokenSource _buttonPressCancellationTokenSource;

        public event EventHandler RotatedClockwise;
        public event EventHandler RotatedCounterClockwise;
        public event EventHandler ButtonPressed;
        public event EventHandler ButtonReleased;
        public event EventHandler ResetEvent; // New event

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

            _buttonHeldDown = false;
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

        private async void ButtonPushed(object sender, PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            _buttonHeldDown = true;
            _buttonPressTimestamp = DateTime.Now;
            ButtonPressed?.Invoke(this, EventArgs.Empty);
            _buttonPressCancellationTokenSource = new CancellationTokenSource();

            try
            {
                await Task.Delay(1500, _buttonPressCancellationTokenSource.Token); // Wait for 3 seconds

                if (_buttonHeldDown)
                {
                    ResetEvent?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (TaskCanceledException)
            {
                // Check the time between button press and release to decide which event to raise.
                var durationPressed = DateTime.Now - _buttonPressTimestamp.Value;

                if (durationPressed < TimeSpan.FromSeconds(1.5))
                {
                    ButtonReleased?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void ButtonReleasedCallback(object sender, PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            if (_buttonHeldDown)
            {
                _buttonPressCancellationTokenSource.Cancel();
                _buttonHeldDown = false;
            }
        }

        public async Task StartListening(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(10, stoppingToken);  // Poll every 10ms, adjust as needed
            }
        }
    }
}

