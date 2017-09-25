

using Kernel.Initialisation;

namespace SearchEngine.Infrastructure
{
    public interface IDocumentContextBuilder<TEvent> : IAutoRegisterAsTransient
    {
        object BuildContext(TEvent ev);
    }
}