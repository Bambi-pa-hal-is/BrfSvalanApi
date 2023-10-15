namespace BrfSvalanApi
{
    public class FileMenu : Menu, IMenu
    {
        public FileMenu(string directoryPath) : base("Select file")
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException($"The directory '{directoryPath}' was not found.");
            }

            var files = Directory.GetFiles(directoryPath);
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                Items.Add(new MenuItem(fileName, () => SelectFileAction(file)));
            }
        }

        private void SelectFileAction(string filePath)
        {
            // Do whatever you want with the selected file.
            // For instance, you could load it, print it, etc.
        }
    }
}
