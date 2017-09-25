using System.Collections.Generic;

namespace Data.Importing.Infrastructure.Contexts
{
    public class ImportContext
    {
        public SourceContext SourceContext { get; private set; }
        public TargetContext TargetContext { get; private set; }

        public ImportContext(SourceContext source, TargetContext target)
        {
            this.SourceContext = source;
            this.TargetContext = target;
        }
    }
}