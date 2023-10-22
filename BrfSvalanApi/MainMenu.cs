using BrfSvalanApi.Display;

namespace BrfSvalanApi
{
    public class MainMenu : IRenderableComponent
    {

        public List<IPipeline> Pipelines { get; set; }

        public DriveManager DriveManager { get; set; }

        public int Selected { get; set; } = 0;

        public MainMenu() {
            Pipelines = new List<IPipeline>();
            DriveManager = new DriveManager();
        }

        public MainMenu(List<IPipeline> pipelines)
        {
            Pipelines = pipelines;
            DriveManager = new DriveManager();
        }

        public void Action(LcdDisplay display)
        {
            var selectedPipeline = Pipelines[Selected];
            var pipelineAsRenderableComponent = selectedPipeline as IRenderableComponent;
            selectedPipeline.Reset();
            if (pipelineAsRenderableComponent != null)
            {
                var canMountUsb = DriveManager.Mount();
                display.ClearDisplay();
                display.Write("Laser usb...");
                if (!canMountUsb)
                {
                    display.SetDisplay(new ErrorDisplay("Kan inte hitta usb."));
                    return;
                }
                display.SetDisplay(pipelineAsRenderableComponent);
            }
        }

        public void Decrease(LcdDisplay display)
        {
            Selected--;
            if (Selected < 0)
            {
                Selected = Pipelines.Count-1;
            }
            Console.WriteLine(Selected);
            display.Update();
        }

        public void Increase(LcdDisplay display)
        {
            Selected++;
            if (Selected > Pipelines.Count - 1)
            {
                Selected = 0;
            }
            Console.WriteLine(Selected);
            display.Update();
        }

        public void Render(LcdDisplay lcdDisplay)
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
            Console.WriteLine("Unloaded mounted usb");
            DriveManager.Unmount();
        }
    }
}
