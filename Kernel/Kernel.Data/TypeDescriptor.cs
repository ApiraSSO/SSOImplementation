using System;
using System.Linq;
using Kernel.Reflection;

namespace Kernel.Data
{
    public class TypeDescriptor
    {
        private string _fullQualifiedName;
        public TypeDescriptor(string fullQualifiedName)
        {
            this._fullQualifiedName = fullQualifiedName;
        }
        public Type Type { get { return this.TypeFromName(); } }
        
        private Type TypeFromName()
        {
            return Type.GetType(this._fullQualifiedName, (an) =>
            {
                if (String.IsNullOrWhiteSpace(this._fullQualifiedName))
                    return null;
                var assembly = AssemblyScanner.ScannableAssemblies.Where(x => x.FullName == an.FullName)
                .FirstOrDefault();
                if (assembly == null)
                    throw new InvalidOperationException(String.Format("Assembly name: {0} can't be resolved.", an));
                return assembly;
            }, (a, s, b) =>
            {
                return a.GetType(s, b);
            });
        }
    }
}