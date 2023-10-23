namespace BrfSvalanApi.Display
{
    public class WindowsDisplay : IDisplay
    {
        private IRenderableComponent? _renderableComponent { get; set; }
        private IRenderableComponent? _defaultRenderableComponent { get; set; }
        public void Write(string value)
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(value);
            Console.SetCursorPosition(0, 10);
        }

        public IRenderableComponent? GetDisplayedComponent()
        {
            return _renderableComponent;
        }

        public void SetDisplay(IRenderableComponent renderableComponent)
        {
            _renderableComponent = renderableComponent;
            Update();
        }

        public void Update()
        {
            if (_renderableComponent != null)
            {
                _renderableComponent.Render(this);
            }
        }

        public void Render(IRenderableComponent? renderableComponent)
        {
            if (renderableComponent != null)
            {
                renderableComponent.Render(this);
            }
        }

        public void Write(int x, int y, string value)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(value);
            Console.SetCursorPosition(0, 10);
        }

        public void ClearDisplay()
        {
            Console.Clear();
        }

        public void Dispose()
        {
        }

        public void SetDefaultDisplay(IRenderableComponent renderableComponent)
        {
            _defaultRenderableComponent = renderableComponent;
        }

        public void GoToDefaultDisplay()
        {
            if (_defaultRenderableComponent != null)
            {
                SetDisplay(_defaultRenderableComponent);
                _defaultRenderableComponent.Load();
                Render(_defaultRenderableComponent);
            }
        }

        public void SetBacklight(bool state)
        {
        }
    }
}
