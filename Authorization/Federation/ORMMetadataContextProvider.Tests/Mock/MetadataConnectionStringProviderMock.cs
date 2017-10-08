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
                DataSource = "NADIM\\SQLEXPRESS",
                InitialCatalog = "SSOConfiguraion",
                IntegratedSecurity = true
            };
        }
    }
}
