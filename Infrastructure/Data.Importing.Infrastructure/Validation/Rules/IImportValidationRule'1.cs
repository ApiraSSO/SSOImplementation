namespace Data.Importing.Infrastructure.Validation.Rules
{
    public interface IImportValidationRule<TEntry, TState> : ISateValidationRule<TState> where TState : ImportState
    {
    }
}