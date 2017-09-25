using System;

namespace Kernel.Cryptography.Validation
{
    public class ValidationRuleDescriptor
    {
        private string _fullQualifiedName;
        public ValidationRuleDescriptor(string fullQualifiedName)
        {
            this._fullQualifiedName = fullQualifiedName;
        }
        public Type Type { get; }
        
        public ValidationScope Scope { get; }
    }
}