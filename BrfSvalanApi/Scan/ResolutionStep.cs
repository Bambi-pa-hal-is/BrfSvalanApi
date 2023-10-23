using BrfSvalanApi.Display;
using BrfSvalanApi.Print;
using static BrfSvalanApi.Print.PrintPipeline;
using static BrfSvalanApi.Scan.ScanPipeline;

namespace BrfSvalanApi.Scan
{
    public class ResolutionStep : IPipelineStep, IRenderableComponent
    {
        public ResolutionStep()
        {
            Resolutions = new List<int>
            {
                1200,600,300,200,100
            };
        }
        public int Resolution { get; set; } = 0;
        private List<int> Resolutions { get; set; }
        public void Action(IDisplay display)
        {
        }

        public void Decrease(IDisplay display)
        {
            Resolution--;
            if (Resolution < 0)
            {
                Resolution = Resolutions.Count-1;
            }
        }

        public void Increase(IDisplay display)
        {
            Resolution++;
            if(Resolution > Resolutions.Count-1)
            {
                Resolution = 0;
            }
        }

        public void Load()
        {
            Console.WriteLine("Loaded ResolutionStep");
        }

        public void Render(IDisplay display)
        {
            display.ClearDisplay();
            display.Write(0, 0, "Valj upplosning");
            display.Write(0, 1, Resolutions[Resolution].ToString());
        }

        public void Reset()
        {
            Resolution = 0;
        }
        public void UpdatePipelineProperties(IPipelineProperties pipelineProperties)
        {
            var printProperties = pipelineProperties as ScanProperties;
            if (printProperties != null)
            {
                printProperties.Resolution = Resolutions[Resolution];
            }
        }
    }
}
