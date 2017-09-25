using System.ComponentModel.DataAnnotations;

namespace Data.Importing.Infrastructure.Validation
{
    public class ImportValidationResult : ValidationResult
    {
        public ImportValidationResult(string errorMessage, Severity severity) : base(errorMessage)
        {
            this.Severity = severity;
        }

        public Severity Severity { get; private set; }
    }
}