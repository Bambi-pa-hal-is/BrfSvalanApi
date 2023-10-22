using BrfSvalanApi.Print;

namespace BrfSvalanApi
{
    public interface IPipelineStep
    {
        public void UpdatePipelineProperties(IPipelineProperties pipelineProperties);
        public void Reset();
    }
}
