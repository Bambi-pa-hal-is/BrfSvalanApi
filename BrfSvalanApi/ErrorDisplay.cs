using BrfSvalanApi.Display;

namespace BrfSvalanApi
{
    public class ErrorDisplay : IRenderableComponent
    {
        public string Error { get; }
        public ErrorDisplay(string error) {
            Error = error;
        }

        public void Action(LcdDisplay display)
        {
            display.GoToDefaultDisplay();
        }

        public void Decrease(LcdDisplay display)
        {
        }

        public void Increase(LcdDisplay display)
        {
        }

        public void Render(LcdDisplay display)
        {
            display.ClearDisplay();
            display.Write(Error);
        }

        public void Load()
        {
        }
    }
}
