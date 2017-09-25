using Kernel.Logging;
namespace Kernel.Logging
{
    public abstract class AbstractLogWriter : ILogWriter
    {
        public abstract void WriteExeption(object o);
    }
}
