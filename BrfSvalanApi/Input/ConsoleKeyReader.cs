namespace BrfSvalanApi.Input
{
    public class ConsoleKeyReader : IInputReader
    {
        public event EventHandler RotatedClockwise;
        public event EventHandler RotatedCounterClockwise;
        public event EventHandler ButtonPressed;
        public event EventHandler ButtonReleased;
        public event EventHandler ResetEvent;

        private HashSet<ConsoleKey> pressedKeys = new HashSet<ConsoleKey>();

        public ConsoleKeyReader()
        {
        }

        public async Task Listen(CancellationToken stoppingToken)
        {

            if (Console.KeyAvailable)
            {
                var keyInfo = Console.ReadKey(true);
                HandleKeyPress(keyInfo.Key);

                // Record the key press
                pressedKeys.Add(keyInfo.Key);
            }
            else
            {
                // No key is currently being pressed, check for key release events
                CheckForReleaseEvents();
            }
            await Task.Delay(1, stoppingToken);

        }

        private void HandleKeyPress(ConsoleKey key)
        {
            if (key == ConsoleKey.Enter)
            {
                ButtonPressed?.Invoke(this, EventArgs.Empty);
            }
            else if (key == ConsoleKey.Escape)
            {
                ResetEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        private void CheckForReleaseEvents()
        {
            if (pressedKeys.Contains(ConsoleKey.RightArrow))
            {
                RotatedClockwise?.Invoke(this, EventArgs.Empty);
                pressedKeys.Remove(ConsoleKey.RightArrow);
            }

            if (pressedKeys.Contains(ConsoleKey.LeftArrow))
            {
                RotatedCounterClockwise?.Invoke(this, EventArgs.Empty);
                pressedKeys.Remove(ConsoleKey.LeftArrow);
            }

            if (pressedKeys.Contains(ConsoleKey.Enter))
            {
                ButtonReleased?.Invoke(this, EventArgs.Empty);
                pressedKeys.Remove(ConsoleKey.Enter);
            }
        }
    }
}
