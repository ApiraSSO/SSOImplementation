namespace Data.Importing.Infrastructure
{
    public enum ImportStages
    {
        Download,
        Deserialise,
        ValidateLevel1,
        Transform,
        ValidateLevel2,
        Commit
    }
}