using BrfSvalanApi.Print;

namespace BrfSvalanApi
{
    public interface IPipeline
    {
        public int PipelineStep { get; set; }
        public void Reset();
        public List<IPipelineStep> Steps { get; set; }
    }
}
