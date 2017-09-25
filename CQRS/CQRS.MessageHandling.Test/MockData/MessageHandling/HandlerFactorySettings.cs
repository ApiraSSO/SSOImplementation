using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kernel.CQRS.MessageHandling;

namespace CQRS.MessageHandling.Test.MockData.MessageHandling
{
    internal class HandlerFactorySettingsMock : IHandlerResolverSettings
    {
        private ICollection<Assembly> _limitAssembliesTo = new List<Assembly>
        {

        };
        public IEnumerable<Assembly> LimitAssembliesTo
        {
            get
            {
                return this._limitAssembliesTo;
            }
        }

        public bool HasCustomAssemlyList
        {
            get
            {
                return this.LimitAssembliesTo != null && this.LimitAssembliesTo.Count() > 0;
            }
        }

        internal void AddAssembly(Assembly assembly)
        {
            this._limitAssembliesTo.Add(assembly);
        }

        internal void ClearList()
        {
            this._limitAssembliesTo.Clear();
        }
    }
}
