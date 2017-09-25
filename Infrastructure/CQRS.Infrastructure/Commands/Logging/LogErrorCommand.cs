namespace CQRS.Infrastructure.Commands.Logging
{
    using System;
    using CQRS.Infrastructure.Messaging;

    [Serializable]
    public class LogErrorCommand : Command
    {
        public LogErrorCommand(Guid id, Guid correlationId) : base(id, correlationId)
        {
        }

        public string Source { get; set; }
        public string MachineName { get; set; }
        public string[] ErrorDetails { get; set; }
	}
}
