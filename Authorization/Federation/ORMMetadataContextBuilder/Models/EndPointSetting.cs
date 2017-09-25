using Kernel.Data;

namespace ORMMetadataContextProvider.Models
{
    public class EndPointSetting : BaseModel
    {
        public virtual Binding Binding { get; set; }
        public string Url { get; set; }
    }
}