using BrfSvalanApi.Display;

namespace BrfSvalanApi
{
    public class InputManager
    {
        private readonly LcdDisplay _display;

        public InputManager(LcdDisplay display)
        {
            _display = display;
        }

        public void HandleRotation(bool clockwise)
        {
            var currentRenderableComponent = _display.GetDisplayedComponent();
            if (currentRenderableComponent is not null)
            {
                if (clockwise)
                {
                    currentRenderableComponent.Increase(_display);
                }
                else
                {
                    currentRenderableComponent.Decrease(_display);
                }
            }
        }

        public void HandleSelection()
        {
            var currentRenderableComponent = _display.GetDisplayedComponent();
            if(currentRenderableComponent is not null)
            {
                currentRenderableComponent.Action(_display);
            }
        }
    }
}
