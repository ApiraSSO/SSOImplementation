using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kernel.Validation
{
    public class ValidationContext : IServiceProvider
    {
        public ValidationContext(object entry)
        {
            this.Entry = entry;
            this.ValidationResult = new List<ValidationResult>();
        }
        public object Entry { get; private set; }
        public ICollection<ValidationResult> ValidationResult { get; private set; }
        object IServiceProvider.GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }
}