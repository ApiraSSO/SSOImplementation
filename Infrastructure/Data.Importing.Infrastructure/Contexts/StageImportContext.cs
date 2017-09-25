namespace Data.Importing.Infrastructure.Contexts
{
    public class StageImportContext
    {
        public StageResult Source { get; private set; }
        public ImportContext ImportContext { get; private set; }

        public StageImportContext(StageResult source, ImportContext context)
        {
            this.Source = source;
            this.ImportContext = context;
        }
    }
}
