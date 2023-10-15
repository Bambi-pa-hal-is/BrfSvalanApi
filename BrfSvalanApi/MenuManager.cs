namespace BrfSvalanApi
{
    public class MenuManager
    {
        private readonly LcdDisplay _display;
        private IMenu? _currentMenu = null;

        public MenuManager(LcdDisplay display)
        {
            _display = display;
        }

        public void SetMenu(Menu menu)
        {
            _currentMenu = menu;
            DisplayCurrentSelection();
        }

        public void HandleRotation(bool clockwise)
        {
            if(_currentMenu is not null)
            {
                if (clockwise)
                {
                    _currentMenu.Increase();
                }
                else
                {
                    _currentMenu.Decrease();
                }
                DisplayCurrentSelection();
            }
        }

        public void HandleSelection()
        {
            if(_currentMenu is not null)
            {
                 _currentMenu.Action(this);
                DisplayCurrentSelection();
            }
        }

        private void DisplayCurrentSelection()
        {
            if(_currentMenu is not null)
            {
                _currentMenu.Display(_display);
            }
        }
    }
}
