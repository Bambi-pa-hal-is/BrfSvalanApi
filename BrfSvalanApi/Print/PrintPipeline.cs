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
                new CopiesStep()
            };
        }

        public void Reset()
        {
            foreach(var step in Steps)
            {
                step.Reset();
                step.UpdatePipelineProperties(Properties);
            }
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
            }
        }

        public void Decrease(LcdDisplay display)
        {
            var currentStep = GetCurrentStep();
            var renderableComponent = currentStep as IRenderableComponent;
            if(renderableComponent != null)
            {
                renderableComponent.Decrease(display);
            }
        }

        public void Action(LcdDisplay display)
        {
            var currentStep = GetCurrentStep();
            var renderableComponent = currentStep as IRenderableComponent;
            if (renderableComponent != null)
            {
                renderableComponent.Action(display);
            }
            currentStep.UpdatePipelineProperties(Properties);
            PipelineStep++;
            display.Update();
        }
    }
}
