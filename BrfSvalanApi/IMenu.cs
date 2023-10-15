namespace BrfSvalanApi
{
    public interface IMenu
    {
        void Increase();
        void Decrease();
        void Action(MenuManager menuManager);
        void Display(LcdDisplay lcdDisplay);
    }
}
