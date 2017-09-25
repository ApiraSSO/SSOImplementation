using System;
using System.Linq;
using Kernel.Reflection;

namespace Kernel.Cryptography.Validation
{
    public class ValidationRuleDescriptor
    {
        private string _fullQualifiedName;
        public ValidationRuleDescriptor(string fullQualifiedName)
        {
            this._fullQualifiedName = fullQualifiedName;
        }
        public Type Type { get { return this.TypeFromName(); } }
        
        public ValidationScope Scope { get; }

        private Type TypeFromName()
        {
            return Type.GetType(this._fullQualifiedName, (an) =>
            {
                return AssemblyScanner.ScannableAssemblies.Where(x => x.FullName == an.FullName)
                .First();
            }, (a, s, b) => 
            {
                return a.GetType(s, b);
            });
        }
    }
}