using System;
using System.Text;

namespace Kernel.Extensions
{
    public static class ExceptionExtensions
    {
        public static string BuildExceptionStringRecursively(this Exception ex)
        {
            if (ex == null)
                return null;

            var stringBuilder = new StringBuilder();

            Func<Exception, bool> BuildInnerExceptionDetail = e =>
            {
                var innerException = e;

                while (innerException != null)
                {

                    stringBuilder.AppendLine("Inner Exception:");

                    AddToExceptionDeails(stringBuilder, innerException);

                    innerException = innerException.InnerException;

                }

                return true;
            };

            AddToExceptionDeails(stringBuilder, ex);

            if (ex is AggregateException)
            {
                var ae = ((AggregateException)ex).Flatten();

                ae.Handle(BuildInnerExceptionDetail);

                return stringBuilder.ToString();
            }

            if (ex.InnerException != null)
                BuildInnerExceptionDetail(ex.InnerException);

            return stringBuilder.ToString();
        }

        private static void AddToExceptionDeails(StringBuilder stringBuilder, Exception exception)
        {
            stringBuilder.AppendFormat("Exception Type: {0}", exception.GetType().Name);

            stringBuilder.AppendLine();

            stringBuilder.AppendFormat("Message: {0}", exception.Message);

            stringBuilder.AppendLine();

            stringBuilder.AppendFormat("Stack: {0}", exception.StackTrace);

            stringBuilder.AppendLine();

            stringBuilder.AppendLine("------------------------------------------------");
        }
    }
}