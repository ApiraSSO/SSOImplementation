using System.Threading.Tasks;
using Data.Importing.Infrastructure.Contexts;

namespace Data.Importing.Infrastructure
{
    public interface IStageProcessor
    {
        StageResultContext GetResult(ImportContext context);
        Task<StageResultContext> GetResultAsync(ImportContext context);
    }
}