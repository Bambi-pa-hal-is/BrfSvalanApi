namespace BrfSvalanApi.Display
{
    public interface IRenderableComponent
    {
        public void Render(IDisplay display);
        void Increase(IDisplay display);
        void Decrease(IDisplay display);
        void Action(IDisplay display);
        void Load();
    }
}
