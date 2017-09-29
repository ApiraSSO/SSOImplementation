using Kernel.Federation.RelyingParty;

namespace Kernel.Federation.Protocols
{
    public interface IAuthnRequestClauseBuilder<TRequest>
    {
        void Build(TRequest request, TenantContext relyingParty);
    }
}