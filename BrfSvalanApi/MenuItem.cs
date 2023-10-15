namespace BrfSvalanApi
{
    public class MenuItem
    {
        public string Name { get; }
        public Action Action { get; }
        public Menu SubMenu { get; }

        public MenuItem(string name, Action action, Menu subMenu = null)
        {
            Name = name;
            Action = action;
            SubMenu = subMenu;
        }
    }

}
