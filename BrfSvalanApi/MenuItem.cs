namespace BrfSvalanApi
{
    public class MenuItem
    {
        public string Name { get; }
        public Action Action { get; }
        public IMenu SubMenu { get; }

        public MenuItem(string name, Action action, IMenu subMenu = null)
        {
            Name = name;
            Action = action;
            SubMenu = subMenu;
        }
    }

}
