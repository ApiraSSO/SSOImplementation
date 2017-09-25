namespace Kernel.Logging
{
	using System;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;

	/// <summary>
	/// Writes information in the event log
	/// </summary>
	/// <seealso cref="System.IDisposable" />
	public class InformationLogEventWriter : IDisposable
    {
		protected readonly Stopwatch Timer = new Stopwatch();
		protected readonly string MethodName;
		public InformationLogEventWriter([CallerMemberName] string methodName = "")
        {
			this.MethodName = methodName;
			LoggerManager.WriteInformationToEventLog(String.Format("{0} entered.", this.MethodName));
			this.Timer.Reset();
			this.Timer.Start();
        }

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Timer.Stop();
				LoggerManager.WriteInformationToEventLog(String.Format("{0} exited. Time Elapsed: {1}", this.MethodName, this.Timer.Elapsed));
			}
		}
	}
}