using BrfSvalanApi.Display;
using BrfSvalanApi.Print;

namespace BrfSvalanApi.Scan
{
    public class ScanPipeline : IPipeline, IRenderableComponent
    {
        //Resoultions 100|200|300|600|1200dpi
        //--format=pnm|tiff|png|jpeg|pdf
        public class ScanProperties : IPipelineProperties
        {
            public int Resolution { get; set; }
            public string FileFormat { get; set; } = "pdf";
            public string Location { get; set; } = "";
        }

        public ScanProperties Properties { get; set; }
        public int PipelineStep { get; set; } = 0;
        public List<IPipelineStep> Steps { get; set; }

        public ScanPipeline()
        {
            Properties = new ScanProperties();
            Steps = new List<IPipelineStep>()
            {
            };
            Reset();
        }

        public void Reset()
        {
            PipelineStep = 0;
            foreach (var step in Steps)
            {
                step.Reset();
                step.UpdatePipelineProperties(Properties);
            }
            Properties = new ScanProperties();
            Steps = new List<IPipelineStep>()
            {
                new FileFormatStep(),
                new ResolutionStep(),
            };
        }

        private IPipelineStep GetCurrentStep()
        {
            return Steps[PipelineStep];
        }

        public void Render(IDisplay display)
        {
            var currentStep = GetCurrentStep();
            var renderableComponent = currentStep as IRenderableComponent;
            display.Render(renderableComponent);
        }

        public void Increase(IDisplay display)
        {
            var currentStep = GetCurrentStep();
            var renderableComponent = currentStep as IRenderableComponent;
            if (renderableComponent != null)
            {
                renderableComponent.Increase(display);
                display.Update();
            }
        }

        public void Decrease(IDisplay display)
        {
            var currentStep = GetCurrentStep();
            var renderableComponent = currentStep as IRenderableComponent;
            if (renderableComponent != null)
            {
                renderableComponent.Decrease(display);
                display.Update();
            }
        }

        public void Action(IDisplay display)
        {
            Console.WriteLine("Next step! " + PipelineStep);
            var currentStep = GetCurrentStep();
            var renderableComponent = currentStep as IRenderableComponent;
            if (renderableComponent != null)
            {
                renderableComponent.Action(display);
            }
            currentStep.UpdatePipelineProperties(Properties);
            PipelineStep++;
            if (PipelineStep >= Steps.Count)
            {
                display.ClearDisplay();
                Properties.Location = DriveManager.GetMountPoint();
                display.Write(0, 0, "Scanning....");
                Console.WriteLine("Scanning!");
                Scanner.Scan(Properties);
                Reset();
                Thread.Sleep(1000);
                display.GoToDefaultDisplay();
                return;
            }
            else
            {
                var nextStep = GetCurrentStep();
                var nextStepRenderableComponent = nextStep as IRenderableComponent;
                if (nextStepRenderableComponent != null)
                {
                    nextStepRenderableComponent.Load();
                }
                display.Update();
            }
            Console.WriteLine("After action step! " + PipelineStep);
        }

        public void Load()
        {
        }
    }
}
