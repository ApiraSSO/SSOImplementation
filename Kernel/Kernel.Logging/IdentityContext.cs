namespace Kernel.Logging
{
	using System;

    public class IdentityContext
    {
        public Guid? UserIdentifier { get; set; }

        public string TenantIdentifier { get; set; }
    }
}