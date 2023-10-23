using BrfSvalanApi.Display;

namespace BrfSvalanApi
{
    using System;

    public class InputManager
    {
        private readonly IDisplay _display;
        private DateTime _lastSelectionTime;

        public InputManager(IDisplay display)
        {
            _display = display;
            _lastSelectionTime = DateTime.MinValue;  // Initialize to a value in the past
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

        public void Reset()
        {
            _display.ClearDisplay();
            _display.GoToDefaultDisplay();
        }

        public void HandleSelection()
        {
            // Check if it's been at least one second since the last call
            if ((DateTime.Now - _lastSelectionTime).TotalSeconds < 0.5)
            {
                return;  // Exit early if it's been less than a second
            }

            var currentRenderableComponent = _display.GetDisplayedComponent();
            if (currentRenderableComponent is not null)
            {
                currentRenderableComponent.Action(_display);
            }

            // Update the last selection time to now
            _lastSelectionTime = DateTime.Now;
        }
    }

}
