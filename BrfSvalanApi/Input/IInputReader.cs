namespace BrfSvalanApi.Input
{
    public interface IInputReader
    {
        public event EventHandler RotatedClockwise;
        public event EventHandler RotatedCounterClockwise;
        public event EventHandler ButtonPressed;
        public event EventHandler ButtonReleased;
        public event EventHandler ResetEvent;
        public Task Listen(CancellationToken stoppingToken);
    }
}
