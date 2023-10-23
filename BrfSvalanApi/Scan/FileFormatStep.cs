using BrfSvalanApi.Display;
using BrfSvalanApi.Print;
using static BrfSvalanApi.Print.PrintPipeline;
using static BrfSvalanApi.Scan.ScanPipeline;

namespace BrfSvalanApi.Scan
{
    public class FileFormatStep : IPipelineStep, IRenderableComponent
    {
        public FileFormatStep()
        {
            FileFormats = new List<string>
            {
                "pdf","jpeg","png","tiff","pnm"
            };
        }
        public int FileFormat { get; set; } = 0;
        private List<string> FileFormats { get; set; }
        public void Action(IDisplay display)
        {
        }

        public void Decrease(IDisplay display)
        {
            FileFormat--;
            if (FileFormat < 0)
            {
                FileFormat = FileFormats.Count-1;
            }
        }

        public void Increase(IDisplay display)
        {
            FileFormat++;
            if(FileFormat > FileFormats.Count-1)
            {
                FileFormat = 0;
            }
        }

        public void Load()
        {
            Console.WriteLine("Loaded ResolutionStep");
        }

        public void Render(IDisplay display)
        {
            display.ClearDisplay();
            display.Write(0, 0, "Valj filformat");
            display.Write(0, 1, FileFormats[FileFormat]);
        }

        public void Reset()
        {
            FileFormat = 0;
        }
        public void UpdatePipelineProperties(IPipelineProperties pipelineProperties)
        {
            var printProperties = pipelineProperties as ScanProperties;
            if (printProperties != null)
            {
                printProperties.FileFormat = FileFormats[FileFormat];
            }
        }
    }
}
