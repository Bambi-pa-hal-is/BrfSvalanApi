namespace BrfSvalanApi
{
    public class CopiesMenu : IMenu
    {
        private int _copies = 1;  // Start with one copy

        public CopiesMenu()
        {
        }

        public void Increase()
        {
            _copies++;
            
        }

        public void Decrease()
        {
            if (_copies > 1)
            {
                _copies--;
            }
        }

        public void Action(MenuManager menuManager)
        {
            throw new NotImplementedException();
        }

        public void Display(LcdDisplay lcdDisplay)
        {
            throw new NotImplementedException();
        }
    }
}
