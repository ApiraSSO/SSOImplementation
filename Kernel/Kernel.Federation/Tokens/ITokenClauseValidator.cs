namespace Kernel.Federation.Tokens
{
    public interface ITokenClauseValidator<TClause>
    {
        void Validate(TClause clause);
    }
}