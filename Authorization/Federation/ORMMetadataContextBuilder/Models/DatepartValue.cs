using Kernel.Data;

namespace ORMMetadataContextProvider.Models
{
    public class DatepartValue : BaseModel
    {
        public Datapart Datepart { get; set; }
        public uint Value { get; set; }
    }
}