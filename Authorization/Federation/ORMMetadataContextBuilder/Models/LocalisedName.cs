using Kernel.Data;

namespace ORMMetadataContextProvider.Models
{
    public class LocalisedName : BaseModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Language { get; set; }
    }
}