using Kernel.Logging;

namespace Federation.Logging
{
    public class WindowsEventLogLogger : AbstractLogger
    {
        public WindowsEventLogLogger(ILogWriter logWriter)
        {
            base.LogWriter = logWriter;
        }

        public override void LogMessage(string m)
        {
            LoggerManager.WriteInformationToEventLog(m);
        }
    }
}