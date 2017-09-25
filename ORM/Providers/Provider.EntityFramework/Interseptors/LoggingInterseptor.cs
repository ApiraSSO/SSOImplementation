using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;

namespace Provider.EntityFramework.Interseptors
{
    internal class LoggingInterseptor : IDbCommandInterceptor
    {
        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext) { }

        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext) { }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext) { }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext) { }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext) { }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext) { }
    }
}
