namespace BrfSvalanApi
{
    public class Menu : IMenu
    {
        public List<MenuItem> Items { get; private set; } = new List<MenuItem>();
        public int _currentIndex = 0;
        private string _title;

        public Menu(string title)
        {
            _title = title;
        }

        public void Next()
        {
            _currentIndex = (_currentIndex + 1) % Items.Count;

        }

        public void Previous()
        {
            _currentIndex = (_currentIndex - 1 + Items.Count) % Items.Count;
        }

        // Implementing the IAdjustableMenu interface
        public virtual void Increase()
        {
            Next();
        }

        public virtual void Decrease()
        {
            Previous();
        }

        public void Action(MenuManager menuManager)
        {
            Items[_currentIndex].Action();
        }

        public void Display(LcdDisplay lcdDisplay)
        {
            lcdDisplay.ClearDisplay();
            lcdDisplay.Write(0, 0, _title);
            lcdDisplay.Write(0, 1, Items[_currentIndex].Name);
        }
    }

}
