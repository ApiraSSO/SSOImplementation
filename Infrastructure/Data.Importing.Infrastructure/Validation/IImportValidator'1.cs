using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Importing.Infrastructure.Validation
{
    public interface IImportValidator<TEntry, TState> : IImportValidator where TState : ImportState
    {
        void Validate(TEntry entry, ICollection<ImportValidationResult> validationResult);
    }
}