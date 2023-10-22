namespace BrfSvalanApi.Display
{
    public interface IRenderableComponent
    {
        public void Render(LcdDisplay display);
        void Increase(LcdDisplay display);
        void Decrease(LcdDisplay display);
        void Action(LcdDisplay display);
        void Load();
    }


}
