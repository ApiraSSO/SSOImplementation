using System.Data.SqlClient;
using Kernel.Data.Connection;

namespace ORMMetadataContextProvider.Tests.Mock
{
    internal class MetadataConnectionStringProviderMock : IConnectionStringProvider
    {
        public SqlConnectionStringBuilder GetConnectionString()
        {
            return new SqlConnectionStringBuilder
            {
                DataSource = "DG-MFB\\SQLEXPRESS_2016",
                InitialCatalog = "SSOConfiguraion_Test",
                IntegratedSecurity = true
            };
        }
    }
}
