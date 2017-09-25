namespace Kernel.Logging
{
	using System;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;

	/// <summary>
	/// Writes a warning in the event log
	/// </summary>
	/// <seealso cref="System.IDisposable" />
	public class WarningLogEventWriter : InformationLogEventWriter
    {
		public WarningLogEventWriter([CallerMemberName] string methodName = "")
			: base(methodName)
		{
		}


		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Timer.Stop();
				LoggerManager.WriteWarningToEventLog(String.Format("{0} exited. Time Elapsed: {1}", this.MethodName, this.Timer.Elapsed));
			}
		}
	}
}