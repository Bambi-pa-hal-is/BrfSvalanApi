using BrfSvalanApi.Display;
using static BrfSvalanApi.Print.PrintPipeline;

namespace BrfSvalanApi.Print
{
    public class SelectDocumentStep : IPipelineStep, IRenderableComponent
    {
        public List<string> Files { get; set; }
        public int SelectFile { get; set; } = 0;

        public SelectDocumentStep()
        {
            if (!Directory.Exists(DriveManager.GetMountPoint()))
            {
                throw new DirectoryNotFoundException($"The directory '{DriveManager.GetMountPoint()}' was not found.");
            }
            Files = new List<string>();
        }

        public void Reset()
        {
            Files = new List<string>();
        }

        public void Action(IDisplay display)
        {
        }

        public void Decrease(IDisplay display)
        {
            SelectFile--;
            if (SelectFile < 0)
            {
                SelectFile = Files.Count-1;
            }
        }

        public void Increase(IDisplay display)
        {
            SelectFile++;
            if(SelectFile >= Files.Count)
            {
                SelectFile = 0;
            }
        }

        private string GetCurrentFile()
        {
            return Files[SelectFile];
        }

        public void Render(IDisplay display)
        {
            if (!Directory.Exists(DriveManager.GetMountPoint()))
            {
                throw new DirectoryNotFoundException($"The directory '{DriveManager.GetMountPoint()}' was not found.");
            }
            Files = Directory.GetFiles(DriveManager.GetMountPoint()).ToList();
            if (Files.Count == 0)
            {
                display.ClearDisplay();
                display.Write(0, 0, "Inga filer hittades");
                Thread.Sleep(1000);
                display.GoToDefaultDisplay();
            }
            else
            {
                var currentFile = GetCurrentFile();
                var fileName = Path.GetFileName(currentFile);
                if (fileName.Length > 16)
                {
                    fileName = fileName.Substring(0, 16);
                }
                display.ClearDisplay();
                display.Write(0, 0, "Vilken fil?");
                display.Write(0, 1, fileName);
            }
        }

        public void UpdatePipelineProperties(IPipelineProperties pipelineProperties)
        {
            var printProperties = pipelineProperties as PrintProperties;
            if (printProperties != null)
            {
                if(Files.Count == 0)
                {
                    printProperties.File = null;
                }
                else
                {
                    printProperties.File = GetCurrentFile();
                }
            }
        }

        public void Load()
        {
            Console.WriteLine("Loaded SelectDocumentStep");
        }
    }
}
