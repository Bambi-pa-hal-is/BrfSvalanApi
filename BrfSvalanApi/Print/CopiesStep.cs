using BrfSvalanApi.Display;
using static BrfSvalanApi.Print.PrintPipeline;

namespace BrfSvalanApi.Print
{
    public class CopiesStep : IPipelineStep, IRenderableComponent
    {
        public int Copies { get; set; } = 1;
        public void Action(LcdDisplay display)
        {
        }

        public void Decrease(LcdDisplay display)
        {
            Copies--;
            if(Copies < 1)
            {
                Copies = 1;
            }
        }

        public void Increase(LcdDisplay display)
        {
            Copies++;
        }

        public void Render(LcdDisplay display)
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
