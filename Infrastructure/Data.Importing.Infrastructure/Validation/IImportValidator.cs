using Kernel.Validation;

namespace Data.Importing.Infrastructure.Validation
{
    public interface IImportValidator : IValidator
    {
        void Validate(ValidationContext context, ImportState state);
    }
}