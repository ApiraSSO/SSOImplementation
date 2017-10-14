using Kernel.Logging;

namespace Federation.Logging
{
    public class WindowsEventLogLogger : AbstractLogger
    {
        public override void LogMessage(string m)
        {
            LoggerManager.WriteInformationToEventLog(m);
        }
    }
}