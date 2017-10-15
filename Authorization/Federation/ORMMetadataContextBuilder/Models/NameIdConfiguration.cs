using Kernel.Data;

namespace ORMMetadataContextProvider.Models
{
    public class NameIdConfiguration : BaseModel
    {
        public bool AllowCreate { get; set; }
        public bool EncryptNameId { get; set; }
        public virtual NameIdFormat DefaultNameIdFormat { get; set; }
    }
}