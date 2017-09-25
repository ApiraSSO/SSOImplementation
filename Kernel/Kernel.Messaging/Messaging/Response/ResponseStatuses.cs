using System;

namespace Kernel.Messaging.Response
{
    /// </summary>
    [Flags]
    public enum ResponseStatuses
    {
        
        Success,
        Failure,
        Exception
    }
}