namespace BrfSvalanApi.Display
{
    public interface IDisplay
    {
        void ClearDisplay();
        void Dispose();
        IRenderableComponent? GetDisplayedComponent();
        void GoToDefaultDisplay();
        void Render(IRenderableComponent? renderableComponent);
        void SetDefaultDisplay(IRenderableComponent renderableComponent);
        void SetDisplay(IRenderableComponent renderableComponent);
        void Update();
        void Write(string value);
        void Write(int x, int y, string value);
    }
}
