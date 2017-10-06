using System.Data.SqlClient;
using Kernel.Data.Connection;

namespace ORMMetadataContextProvider.Tests.Mock
{
    internal class GlobalConnectionStringProviderMock : IConnectionStringProvider
    {
        public SqlConnectionStringBuilder GetConnectionString()
        {
            return new SqlConnectionStringBuilder
            {
                DataSource = "NADIM\\SQLEXPRESS",
                InitialCatalog = "SSOConfiguration",
                IntegratedSecurity = true
            };
        }
    }
}
