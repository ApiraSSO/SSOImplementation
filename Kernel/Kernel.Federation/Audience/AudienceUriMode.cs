namespace Kernel.Federation.Audience
{
    public enum AudienceUriMode
    {
        Never = 0,
        //
        // Summary:
        //     Always.
        Always = 1,
        //
        // Summary:
        //     Only when the security token's key is of type BearerKey and there are no proof
        //     of possession keys in the security token.
        BearerKeyOnly = 2
    }
}