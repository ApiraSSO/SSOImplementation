using System;
using System.Threading.Tasks;
using Data.Importing.Infrastructure.Contexts;

namespace Data.Importing.Infrastructure
{
    public interface IStageProcessor<TStage> : IStageProcessor where  TStage : struct, IConvertible
    {
        ImportStage<TStage> Stage { get; }
    }
}