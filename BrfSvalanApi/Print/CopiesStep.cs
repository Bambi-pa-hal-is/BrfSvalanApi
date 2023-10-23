using BrfSvalanApi.Display;
using static BrfSvalanApi.Print.PrintPipeline;

namespace BrfSvalanApi.Print
{
    public class CopiesStep : IPipelineStep, IRenderableComponent
    {
        public int Copies { get; set; } = 1;
        public void Action(IDisplay display)
        {
        }

        public void Decrease(IDisplay display)
        {
            Copies--;
            if(Copies < 1)
            {
                Copies = 1;
            }
        }

        public void Increase(IDisplay display)
        {
            Copies++;
        }

        public void Load()
        {
            Console.WriteLine("Loaded SelectDocumentStep");
        }

        public void Render(IDisplay display)
        {
            display.ClearDisplay();
            display.Write(0, 0, "Hur manga kopior?");
            display.Write(0, 1, Copies.ToString());
        }

        public void Reset()
        {
            Copies = 1;
        }
        public void UpdatePipelineProperties(IPipelineProperties pipelineProperties)
        {
            var printProperties = pipelineProperties as PrintProperties;
            if(printProperties != null)
            {
                printProperties.Copies = Copies;
            }
        }
    }
}
