using System.Security.Cryptography.X509Certificates;
using Kernel.Data;

namespace ORMMetadataContextProvider.Models
{
    public class StoreSearchCriterion : BaseModel
    {
        public string SearchCriteria { get; set; }
        public X509FindType SearchCriteriaType { get; set; }
    }
}