using System.Threading.Tasks;

namespace EstudosPipelinePattern.Main.BlockingCollection.Contracts
{
    public interface IAwaitablePipeline<TOutPut>
    {
        Task<TOutPut> Execute(object input);
    }
}