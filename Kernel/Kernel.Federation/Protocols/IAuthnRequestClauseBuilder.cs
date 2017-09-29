﻿using Kernel.Federation.FederationPartner;

namespace Kernel.Federation.Protocols
{
    public interface IAuthnRequestClauseBuilder<TRequest>
    {
        void Build(TRequest request, FederationPartner.FederationPartnerContext relyingParty);
    }
}