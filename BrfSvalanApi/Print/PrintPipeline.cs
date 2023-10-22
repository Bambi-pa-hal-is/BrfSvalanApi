using BrfSvalanApi.Display;

namespace BrfSvalanApi.Print
{
    public class PrintPipeline : IPipeline, IRenderableComponent
    {
        public class PrintProperties : IPipelineProperties
        {
            public string? File { get; set; }
            public int Copies { get; set; }
        }

        public PrintProperties Properties { get; set; }
        public int PipelineStep { get; set; } = 0;
        public List<IPipelineStep> Steps { get; set; }

        public PrintPipeline()
        {
            Properties = new PrintProperties();
            Steps = new List<IPipelineStep>()
            {
                new SelectDocumentStep(),
                new CopiesStep(),
            };
        }

        public void Reset()
        {
            PipelineStep = 0;
            foreach (var step in Steps)
            {
                step.Reset();
                step.UpdatePipelineProperties(Properties);
            }
            Properties = new PrintProperties();
            Steps = new List<IPipelineStep>()
            {
                new CopiesStep(),
                new SelectDocumentStep(),
            };
        }

        private IPipelineStep GetCurrentStep()
        {
            return Steps[PipelineStep];
        }

        public void Render(LcdDisplay display)
        {
            var currentStep = GetCurrentStep();
            var renderableComponent = currentStep as IRenderableComponent;
            display.Render(renderableComponent);
        }

        public void Increase(LcdDisplay display)
        {
            var currentStep = GetCurrentStep();
            var renderableComponent = currentStep as IRenderableComponent;
            if (renderableComponent != null)
            {
                renderableComponent.Increase(display);
                display.Update();
            }
        }

        public void Decrease(LcdDisplay display)
        {
            var currentStep = GetCurrentStep();
            var renderableComponent = currentStep as IRenderableComponent;
            if(renderableComponent != null)
            {
                renderableComponent.Decrease(display);
                display.Update();
            }
        }

        public void Action(LcdDisplay display)
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
                Console.WriteLine("PRINTING!");
                CupsPrinter.Print(Properties);
                Reset();
                display.Write(0, 0, "printing....");
                Thread.Sleep(1000);
                display.GoToDefaultDisplay();
                return;
            }
            else
            {
                var nextStep = GetCurrentStep();
                var nextStepRenderableComponent = nextStep as IRenderableComponent;
                if(nextStepRenderableComponent != null)
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
