using BrfSvalanApi.Display;

namespace BrfSvalanApi
{
    public class MainMenu : IRenderableComponent
    {

        public List<IPipeline> Pipelines { get; set; }


        public int Selected { get; set; } = 0;

        public MainMenu() {
            Pipelines = new List<IPipeline>();
        }

        public MainMenu(List<IPipeline> pipelines)
        {
            Pipelines = pipelines;
        }

        public void Action(IDisplay display)
        {
            var selectedPipeline = Pipelines[Selected];
            var pipelineAsRenderableComponent = selectedPipeline as IRenderableComponent;
            selectedPipeline.Reset();
            if (pipelineAsRenderableComponent != null)
            {
                var isUsbMounted = DriveManager.Mount();
                display.ClearDisplay();
                display.Write("Laser usb...");
                Thread.Sleep(500);
                if (!isUsbMounted)
                {
                    display.SetDisplay(new ErrorDisplay("Kan inte hitta usb."));
                    return;
                }
                display.SetDisplay(pipelineAsRenderableComponent);
            }
        }

        public void Decrease(IDisplay display)
        {
            Selected--;
            if (Selected < 0)
            {
                Selected = Pipelines.Count-1;
            }
            display.Update();
        }

        public void Increase(IDisplay display)
        {
            Selected++;
            if (Selected > Pipelines.Count - 1)
            {
                Selected = 0;
            }
            display.Update();
        }

        public void Render(IDisplay lcdDisplay)
        {
            lcdDisplay.ClearDisplay();
            if(Selected == 0)
            {
                lcdDisplay.Write(0, 0, "Vad vill du gora?");
                lcdDisplay.Write(0, 1, ">Print   Scan");
            }
            else
            {
                lcdDisplay.Write(0, 0, "Vad vill du gora?");
                lcdDisplay.Write(0, 1, " Print  >Scan");
            }
        }

        public void Load()
        {
        }
    }
}
