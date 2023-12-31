﻿using BrfSvalanApi.Display;

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
            Properties = new PrintProperties();
            Steps = new List<IPipelineStep>()
            {
                new IsPrinterStarted(),
                new SelectDocumentStep(),
                new CopiesStep(),
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
            if(renderableComponent != null)
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
                Console.WriteLine("PRINTING!");
                display.Write(0, 0, "Printing...");
                var result = CupsPrinter.Print(Properties);
                if(!result)
                {
                    display.ClearDisplay();
                    display.Write(0, 0, "Skrivaren ar");
                    display.Write(0, 1, "offline");
                }
                Reset();
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
