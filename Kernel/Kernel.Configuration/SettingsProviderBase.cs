namespace Kernel.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public abstract class SettingsProviderBase
	{
		public abstract IList<ValidationResult> ValidationResult { get; }

		public abstract bool IsValid { get; }

		public bool HasError { get { return Error != null; } }

		public abstract Exception Error { get; }
	}
}