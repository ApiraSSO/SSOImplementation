using System;
using System.Collections.Generic;
using System.Linq;
using Federation.Protocols.Request;
using Federation.Protocols.Request.ClauseBuilders;
using Kernel.Federation.Protocols;
using Kernel.Reflection;
using Shared.Federtion.Models;

namespace Federation.Protocols.Test.Mock
{
    internal class AuthnRequestBuildersFactoryMock
    {
        internal static Func<IEnumerable<IAuthnRequestClauseBuilder<AuthnRequest>>> GetBuildersFactory()
        {
            return () => ReflectionHelper.GetAllTypes(new[] { typeof(ClauseBuilder).Assembly }, t => AuthnRequestHelper.Condition(t))
                .Select(x => (IAuthnRequestClauseBuilder<AuthnRequest>)Activator.CreateInstance(x));
        }
    }
}