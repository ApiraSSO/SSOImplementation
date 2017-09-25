using System;

namespace Kernel.Federation
{
    public interface IRelayState
    {
        object AdditionalState { get; }
        Uri ResourceURI { get; }
    }
}