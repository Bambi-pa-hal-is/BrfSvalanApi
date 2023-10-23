using BrfSvalanApi.Display;
using BrfSvalanApi.Print;

namespace BrfSvalanApi
{
    public class IsPrinterStarted : IPipelineStep, IRenderableComponent
    {
        public void Action(IDisplay display)
        {
        }

        public void Decrease(IDisplay display)
        {
        }

        public void Increase(IDisplay display)
        {
        }

        public void Load()
        {
        }

        public void Render(IDisplay display)
        {
            display.ClearDisplay();
            display.Write(0, 0, "Kolla att");
            display.Write(0, 1, "skrivaren ar pa");
        }

        public void Reset()
        {
        }

        public void UpdatePipelineProperties(IPipelineProperties pipelineProperties)
        {
        }
    }
}
