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
            var directoryPath = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException($"The directory '{directoryPath}' was not found.");
            }
            Files = Directory.GetFiles(directoryPath).ToList();
        }

        public void Reset()
        {
            var directoryPath = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException($"The directory '{directoryPath}' was not found.");
            }
            Files = Directory.GetFiles(directoryPath).ToList();
        }

        public void Action(LcdDisplay display)
        {
        }

        public void Decrease(LcdDisplay display)
        {
            SelectFile--;
            if (SelectFile < 0)
            {
                SelectFile = Files.Count-1;
            }

        }

        public void Increase(LcdDisplay display)
        {
            SelectFile++;
            if(SelectFile > Files.Count)
            {
                SelectFile = 0;
            }
        }

        private string GetCurrentFile()
        {
            return Files[SelectFile];
        }

        public void Render(LcdDisplay display)
        {
            var currentFile = GetCurrentFile();
            var fileName = Path.GetFileName(currentFile);
            if(fileName.Length > 16)
            {
                fileName = fileName.Substring(0, 16);
            }
            display.ClearDisplay();
            display.Write(0, 0, "Vilken fil?");
            display.Write(0, 1, GetCurrentFile());
        }

        public void UpdatePipelineProperties(IPipelineProperties pipelineProperties)
        {
            var printProperties = pipelineProperties as PrintProperties;
            if (printProperties != null)
            {
                printProperties.File = GetCurrentFile();
            }
        }
    }
}
