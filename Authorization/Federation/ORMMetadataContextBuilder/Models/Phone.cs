using Kernel.Data;

namespace ORMMetadataContextProvider.Models
{
    public class Phone : BaseModel
    {
        public PhoneType Type {get;set;}
        public string Number { get; set; }
    }
}