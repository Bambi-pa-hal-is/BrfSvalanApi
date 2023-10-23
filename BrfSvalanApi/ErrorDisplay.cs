using BrfSvalanApi.Display;

namespace BrfSvalanApi
{
    public class ErrorDisplay : IRenderableComponent
    {
        public string Error { get; }
        public ErrorDisplay(string error) {
            Error = error;
        }

        public void Action(IDisplay display)
        {
            display.GoToDefaultDisplay();
        }

        public void Decrease(IDisplay display)
        {
        }

        public void Increase(IDisplay display)
        {
        }

        public void Render(IDisplay display)
        {
            display.ClearDisplay();
            display.Write(Error);
        }

        public void Load()
        {
        }
    }
}
